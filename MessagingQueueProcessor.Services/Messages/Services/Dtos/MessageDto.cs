using MessagingQueueProcessor.Data.Enums;

namespace MessagingQueueProcessor.Services.Messages.Services.Dtos
{
    public sealed record MessageDto
    {
        public Guid Id { get; set; }

        public required MessageType Type { get; init; }

        public required MessageStatus Status { get; init; }

        public required string Payload { get; init; }

        public required int RetryCount { get; init; }
    }
}
