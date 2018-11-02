using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


// Boilerplate from the ChangeFeedProcessor documentation.
public class DocumentFeedObserver : IChangeFeedObserver
{
    public static Action<Document> ChatDocumentReceived;
    public static Action<Document> FlashcardDocumentReceived;

    public DocumentFeedObserver()
    {
    }

    public Task OpenAsync(IChangeFeedObserverContext context)
    {
        return Task.CompletedTask;
    }

    public Task CloseAsync(IChangeFeedObserverContext context, ChangeFeedObserverCloseReason reason)
    {
        return Task.CompletedTask;
    }

    public Task ProcessChangesAsync(IChangeFeedObserverContext context, IReadOnlyList<Document> docs, CancellationToken cancellationToken)
    {
        // The parameter docs is the collection item reported as being new/changed by the Observer.
        foreach (Document doc in docs)
        {
            // The doc is converted to json for processing purposes.
            var json = JsonConvert.SerializeObject(doc);

            // The json is converted to a JObject for the purpose of accessing its child properties.
            JObject o = JObject.Parse(json);

            // Flashcards and Messages contain a property declaring their type, "ObjType".
            string docType = (string)o["ObjType"];

            // The incoming doc must have its type checked so the correct dependency injection is made. Without this step, 
            // every changed collection item is sent to both Chat and Flashcard ViewModels.
            if (docType == "Msg")
            {
                ChatDocumentReceived(doc);
            }
            else if (docType == "Card")
            {
                FlashcardDocumentReceived(doc);
            }
            else
            {
                Console.WriteLine("Received unknown doc");
            }
        }

        return Task.CompletedTask;
    }

}