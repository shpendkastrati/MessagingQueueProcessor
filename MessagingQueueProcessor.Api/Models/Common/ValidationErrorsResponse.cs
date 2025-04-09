namespace MessagingQueueProcessor.Api.Models.Common
{
    public sealed record ValidationErrorsResponse
    {
        public IReadOnlyCollection<string> Messages { get; init; } = null!;
    }
}
