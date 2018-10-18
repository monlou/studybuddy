using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Text;
using StudyBuddy.Helpers;
using StudyBuddy.Models;
using System.Threading.Tasks;



namespace StudyBuddy.Services
{
    class ChatDBService
    {
        DocumentClient ChatClient;
        Uri CollectionLink;

        public ChatDBService()
        {
            ChatClient = new DocumentClient(new Uri(Keys.CosmosDBUri), Keys.CosmosDBKey);
            CollectionLink = UriFactory.CreateDocumentCollectionUri("Chat", "Messages");
        }


        public async Task UploadMessage(Message message)
        {
            Console.WriteLine("Hit UploadMessage");
            await ChatClient.CreateDocumentAsync(CollectionLink, message);
            Console.WriteLine("Created a new message in " + CollectionLink);

        }

    }
}
