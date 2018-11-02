using Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing;

// Boilerplate from the ChangeFeedProcessor documentation.
public class DocumentFeedObserverFactory : IChangeFeedObserverFactory
{
    public DocumentFeedObserverFactory()
    {
    }

    public IChangeFeedObserver CreateObserver()
    {
        DocumentFeedObserver newObserver = new DocumentFeedObserver();
        return newObserver as IChangeFeedObserver;
    }
}