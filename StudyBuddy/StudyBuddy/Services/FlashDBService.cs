﻿using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Text;
using StudyBuddy.Helpers;
using StudyBuddy.Models;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.ChangeFeedProcessor;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Azure.Documents.Linq;
using StudyBuddy.ViewModels;

namespace StudyBuddy.Services
{
    class FlashDBService
    {
        public static event Action<CardDeck> FlashcardReceived;

        static DocumentClient FlashClient;
        static Uri CollectionLink;

        private static List<CardDeck> _flashcards;


        public FlashDBService()
        {
            FlashClient = new DocumentClient(new Uri(Keys.CosmosDBUri), Keys.CosmosDBKey);
            CollectionLink = UriFactory.CreateDocumentCollectionUri(GroupSelectionPageViewModel.SelectedDBName, "Flashcards");
            DocumentFeedObserver.FlashcardDocumentReceived += Observer_DocumentReceived;

        }

        // When the dependency injection from the Observer is fulfilled this method is called to convert it into an appropriate form
        // and then make another dependency injection which will send the data to the ViewModel.
        private void Observer_DocumentReceived(Document doc)
        {
            var json = JsonConvert.SerializeObject(doc);
            var deck = JsonConvert.DeserializeObject<CardDeck>(json);

            FlashcardReceived?.Invoke(deck);
        }


        public async static Task UploadFlashCard(CardDeck deck)
        {
            await FlashClient.CreateDocumentAsync(CollectionLink, deck);
            Console.WriteLine("Created a new flashcard deck in " + CollectionLink);

        }

        // Whenever the page is constructed, it will call this method to query the collection for 
        // flashcards received while the client was inactive.
        public async static Task<List<CardDeck>> LoadFlashcards()
        {
            _flashcards = new List<CardDeck>();

            try
            {
                var query = FlashClient.CreateDocumentQuery<CardDeck>(CollectionLink)
                                  .AsDocumentQuery();

                while (query.HasMoreResults)
                {
                    _flashcards.AddRange(await query.ExecuteNextAsync<CardDeck>());
                }
            }
            catch (DocumentClientException ex)
            {
                Debug.WriteLine("Error: ", ex.Message);
            }

            return _flashcards;
        }

        // Boilerplate from the ChangeFeedProcessor library.
        public async Task RunChangeFeedHostAsync()
        {
            string hostName = "FlashHost" + DateTime.Now.Ticks.ToString();

            DocumentCollectionInfo documentCollectionInfo = new DocumentCollectionInfo
            {
                Uri = new Uri(Keys.CosmosDBUri),
                MasterKey = Keys.CosmosDBKey,
                DatabaseName = GroupSelectionPageViewModel.SelectedDBName,
                CollectionName = "Flashcards"
            };

            DocumentCollectionInfo leaseCollectionInfo = new DocumentCollectionInfo
            {
                Uri = new Uri(Keys.CosmosDBUri),
                MasterKey = Keys.CosmosDBKey,
                DatabaseName = GroupSelectionPageViewModel.SelectedDBName,
                CollectionName = "FlashLease"
            };
            DocumentFeedObserverFactory docObserverFactory = new DocumentFeedObserverFactory();
            ChangeFeedProcessorOptions feedProcessorOptions = new ChangeFeedProcessorOptions();

            feedProcessorOptions.LeaseRenewInterval = TimeSpan.FromSeconds(15);
            feedProcessorOptions.StartFromBeginning = true;
            ChangeFeedProcessorBuilder builder = new ChangeFeedProcessorBuilder();
            builder
                .WithHostName(hostName)
                .WithFeedCollection(documentCollectionInfo)
                .WithLeaseCollection(leaseCollectionInfo)
                .WithProcessorOptions(feedProcessorOptions)
                .WithObserverFactory(new DocumentFeedObserverFactory());

            var result = await builder.BuildAsync();
            await result.StartAsync();
        }


    }
}
