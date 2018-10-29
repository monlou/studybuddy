using Microsoft.Azure.Documents.Client;
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

        private void Observer_DocumentReceived(Document doc)
        {
            var json = JsonConvert.SerializeObject(doc);
            var deck = JsonConvert.DeserializeObject<CardDeck>(json);
            //Console.WriteLine("FLASHCARDS JUST RECIEVED A NEW DOCUMENT");

            //if (msg.UserId == this.settings.UserId)
            //    return;


            FlashcardReceived?.Invoke(deck);
        }


        public async static Task UploadFlashCard(CardDeck deck)
        {
            await FlashClient.CreateDocumentAsync(CollectionLink, deck);
            Console.WriteLine("Created a new flashcard deck in " + CollectionLink);

        }

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


        public async Task RunChangeFeedHostAsync()
        {

            string hostName = "FlashHost" + DateTime.Now.Ticks.ToString();


            // monitored collection info
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

            // ie. customizing lease renewal interval to 15 seconds
            // can customize LeaseRenewInterval, LeaseAcquireInterval, LeaseExpirationInterval, FeedPollDelay
            feedProcessorOptions.LeaseRenewInterval = TimeSpan.FromSeconds(15);
            feedProcessorOptions.StartFromBeginning = true;
            ChangeFeedProcessorBuilder builder = new ChangeFeedProcessorBuilder();
            builder
                .WithHostName(hostName)
                .WithFeedCollection(documentCollectionInfo)
                .WithLeaseCollection(leaseCollectionInfo)
                .WithProcessorOptions(feedProcessorOptions)
                .WithObserverFactory(new DocumentFeedObserverFactory());

            //    .WithObserver<DocumentFeedObserver>();  or just pass a observer

            var result = await builder.BuildAsync();
            await result.StartAsync();

            //await result.StopAsync();
        }


    }
}
