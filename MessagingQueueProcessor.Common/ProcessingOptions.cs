namespace MessagingQueueProcessor.Common
{
    public sealed record ProcessingOptions
    {
        public int ThrottleDelay { get; init; } = default!;
    }
}
