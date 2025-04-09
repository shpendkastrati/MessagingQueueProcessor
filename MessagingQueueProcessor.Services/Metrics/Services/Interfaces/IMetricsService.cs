namespace MessagingQueueProcessor.Services.Metrics.Services.Interfaces
{
    public interface IMetricsService
    {
        int ProcessedCount { get; }

        int ErrorCount { get; }

        void IncrementProcessed();

        void IncrementError();
    }
}
