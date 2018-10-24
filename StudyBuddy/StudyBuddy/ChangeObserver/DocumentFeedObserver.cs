using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing;
using Microsoft.Azure.Documents.Client;

public class DocumentFeedObserver : IChangeFeedObserver
{
    public static Action<Document> DocumentReceived;
    private static int totalDocs = 0;

    public DocumentFeedObserver()
    {
    }

    public Task OpenAsync(IChangeFeedObserverContext context)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Observer opened for partition Key Range: {0}", context.PartitionKeyRangeId);
        return Task.CompletedTask;
    }

    public Task CloseAsync(IChangeFeedObserverContext context, ChangeFeedObserverCloseReason reason)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Observer closed, {0}", context.PartitionKeyRangeId);
        Console.WriteLine("Reason for shutdown, {0}", reason);
        return Task.CompletedTask;
    }

    public Task ProcessChangesAsync(IChangeFeedObserverContext context, IReadOnlyList<Document> docs, CancellationToken cancellationToken)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Change feed: PartitionId {0} total {1} doc(s)", context.PartitionKeyRangeId, Interlocked.Add(ref totalDocs, docs.Count));
        foreach (Document doc in docs)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(doc.ToString());
            DocumentReceived(doc);
        }

        return Task.CompletedTask;
    }
}