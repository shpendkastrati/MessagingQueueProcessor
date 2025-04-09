using MessagingQueueProcessor.Data.Enums;

namespace MessagingQueueProcessor.Services.Messages.QueueService.Dtos
{
    public sealed record CreateMessageDto
    {
        public Guid Id { get; set; }

        public required MessageType Type { get; init; }

        public required MessageStatus Status { get; init; }

        public required string Payload { get; init; }

        public required int RetryCount { get; init; }
    }
}
