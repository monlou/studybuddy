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

namespace StudyBuddy.Services
{
    class GroupDBService
    {
        static DocumentClient ChatClient;
        static Uri CollectionLink;

        public GroupDBService()
        {
            ChatClient = new DocumentClient(new Uri(Keys.CosmosDBUri), Keys.CosmosDBKey);
            CollectionLink = UriFactory.CreateDocumentCollectionUri("Master", "Groups");
        }

        public async static Task UploadGroup(Group group)
        {
            Console.WriteLine("Hit UploadMessage");
            await ChatClient.CreateDocumentAsync(CollectionLink, group);
            Console.WriteLine("Created a new group in " + CollectionLink);
        }
    }
}
