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
using Microsoft.Azure.Documents.Linq;
using System.Diagnostics;

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
            CollectionLink = UriFactory.CreateDocumentCollectionUri("Chat", "Messages");
            DocumentFeedObserver.ChatDocumentReceived += Observer_DocumentReceived;

        }

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
            Console.WriteLine("Hit UploadMessage");
            await ChatClient.CreateDocumentAsync(CollectionLink, message);
            Console.WriteLine("Created a new message in " + CollectionLink);

        }

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

        public async Task RunChangeFeedHostAsync()
        {

            string hostName = "ChatHost" + DateTime.Now.Ticks.ToString();


            // monitored collection info
            DocumentCollectionInfo documentCollectionInfo = new DocumentCollectionInfo
            {
                Uri = new Uri(Keys.CosmosDBUri),
                MasterKey = Keys.CosmosDBKey,
                DatabaseName = "Chat",
                CollectionName = "Messages"
            };


            DocumentCollectionInfo leaseCollectionInfo = new DocumentCollectionInfo
            {
                Uri = new Uri(Keys.CosmosDBUri),
                MasterKey = Keys.CosmosDBKey,
                DatabaseName = "Chat",
                CollectionName = "Lease"
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
