namespace MessagingQueueProcessor.Api.Models.Metrics
{
    public class DashboardViewModel
    {
        public int ProcessedMessages { get; set; }

        public int ErrorCount { get; set; }

        public int QueueDepth { get; set; }
    }
}
