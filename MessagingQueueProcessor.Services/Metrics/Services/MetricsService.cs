using MessagingQueueProcessor.Services.Metrics.Services.Interfaces;

namespace MessagingQueueProcessor.Services.Metrics.Services
{
    public class MetricsService : IMetricsService
    {
        private int _processedCount;
        private int _errorCount;

        public int ProcessedCount => _processedCount;
        public int ErrorCount => _errorCount;

        public void IncrementProcessed() => Interlocked.Increment(ref _processedCount);

        public void IncrementError() => Interlocked.Increment(ref _errorCount);
    }
}
