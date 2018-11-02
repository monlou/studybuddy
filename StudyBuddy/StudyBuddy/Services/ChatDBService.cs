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
using Microsoft.Azure.Documents.Linq;
using System.Diagnostics;
using StudyBuddy.ViewModels;

namespace StudyBuddy.Services
{
    class ChatDBService
    {
        public static event Action<Message> MessageReceived;

        private static List<Message> _messages;

        static DocumentClient ChatClient;
        static Uri CollectionLink;

        public ChatDBService()
        {
            ChatClient = new DocumentClient(new Uri(Keys.CosmosDBUri), Keys.CosmosDBKey);
            CollectionLink = UriFactory.CreateDocumentCollectionUri(GroupSelectionPageViewModel.SelectedDBName, "Messages");
            DocumentFeedObserver.ChatDocumentReceived += Observer_DocumentReceived;
        }

        // When the dependency injection from the Observer is fulfilled this method is called to convert it into an appropriate form
        // and then make another dependency injection which will send the data to the ViewModel.
        private void Observer_DocumentReceived(Document doc)
        {
            var json = JsonConvert.SerializeObject(doc);
            var message = JsonConvert.DeserializeObject<Message>(json);
            //Console.WriteLine("CHAT JUST RECIEVED A NEW DOCUMENT");

            //if (msg.UserId == this.settings.UserId)
            //    return;


            MessageReceived?.Invoke(message);
        }


        public async static Task UploadMessage(Message message)
        {
            await ChatClient.CreateDocumentAsync(CollectionLink, message);
        }

        // Whenever the page is constructed, it will call this method to query the collection for 
        // flashcards received while the client was inactive.
        public async static Task<List<Message>> LoadMessages()
        {
            _messages = new List<Message>();

            try
            {
                var query = ChatClient.CreateDocumentQuery<Message>(CollectionLink)
                                  .AsDocumentQuery();

                while (query.HasMoreResults)
                {
                    _messages.AddRange(await query.ExecuteNextAsync<Message>());
                }
            }
            catch (DocumentClientException ex)
            {
                Debug.WriteLine("Error: ", ex.Message);
            }

            return _messages;
        }


        // Boilerplate from the ChangeFeedProcessor library.
        public async Task RunChangeFeedHostAsync()
        {

            string hostName = "ChatHost" + DateTime.Now.Ticks.ToString();


            // The collection to be observed is registered here.
            DocumentCollectionInfo documentCollectionInfo = new DocumentCollectionInfo
            {
                Uri = new Uri(Keys.CosmosDBUri),
                MasterKey = Keys.CosmosDBKey,
                DatabaseName = GroupSelectionPageViewModel.SelectedDBName,
                CollectionName = "Messages"
            };

            // The lease is a collection where the changes are temporary stored.
            DocumentCollectionInfo leaseCollectionInfo = new DocumentCollectionInfo
            {
                Uri = new Uri(Keys.CosmosDBUri),
                MasterKey = Keys.CosmosDBKey,
                DatabaseName = GroupSelectionPageViewModel.SelectedDBName,
                CollectionName = "Lease"
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

            // A new ChangeFeedProcessor is built and then run asynchronously.
            var result = await builder.BuildAsync();
            await result.StartAsync();
        }


    }
}
