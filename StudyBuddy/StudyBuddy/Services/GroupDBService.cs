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

        // The "master" DB houses Group models, which house the names, codes and creator identities of each StudyBuddy group.
        // These are tied to actual DBs, allowing for multiple servers with independent communication channels.
        public GroupDBService()
        {
            GroupClient = new DocumentClient(new Uri(Keys.CosmosDBUri), Keys.CosmosDBKey);
            CollectionLink = UriFactory.CreateDocumentCollectionUri("Master", "Groups");
        }

        public static async Task CreateDatabase(string databaseName)
        {
            try
            {
                // Using the parameters, if it does not already exist by the given name, a new DB is created.
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
                // Using the parameters, if it does not already exist by the given name, a new collection is created with 400 throughput.
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
            await GroupClient.CreateDocumentAsync(CollectionLink, group);
        }

        // Whenever the page is constructed, it will call this method to query the collection for 
        // groups created while the client was inactive.
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
