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

namespace StudyBuddy.Services
{
    class GroupDBService
    {
        static DocumentClient GroupClient;
        static Uri CollectionLink;

        private static List<Group> _groups;

        public GroupDBService()
        {
            GroupClient = new DocumentClient(new Uri(Keys.CosmosDBUri), Keys.CosmosDBKey);
            CollectionLink = UriFactory.CreateDocumentCollectionUri("Master", "Groups");
        }

        public static async Task CreateDatabase(string databaseName)
        {
            try
            {
                await GroupClient.CreateDatabaseIfNotExistsAsync(new Database
                {
                    Id = databaseName
                });
            }
            catch (DocumentClientException ex)
            {
                Debug.WriteLine("Error: ", ex.Message);
            }
        }

        public static async Task CreateDocumentCollection(string databaseName, string collectionName)
        {
            try
            {
                // Create collection with 400 RU/s
                await GroupClient.CreateDocumentCollectionIfNotExistsAsync(
                    UriFactory.CreateDatabaseUri(databaseName),
                    new DocumentCollection
                    {
                        Id = collectionName
                    },
                    new RequestOptions
                    {
                        OfferThroughput = 400
                    });
            }
            catch (DocumentClientException ex)
            {
                Debug.WriteLine("Error: ", ex.Message);
            }
        }

        public async static Task UploadGroup(Group group)
        {
            Console.WriteLine("Hit UploadMessage");
            await GroupClient.CreateDocumentAsync(CollectionLink, group);
            Console.WriteLine("Created a new group in " + CollectionLink);
        }

        public async static Task<List<Group>> LoadGroups()
        {
            _groups = new List<Group>();

            try
            {
                var query = GroupClient.CreateDocumentQuery<Group>(CollectionLink)
                                  .AsDocumentQuery();

                while (query.HasMoreResults)
                {
                    _groups.AddRange(await query.ExecuteNextAsync<Group>());
                }
            }
            catch (DocumentClientException ex)
            {
                Debug.WriteLine("Error: ", ex.Message);
            }

            return _groups;
        }

    }
}
