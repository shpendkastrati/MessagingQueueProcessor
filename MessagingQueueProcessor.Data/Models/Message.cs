using MessagingQueueProcessor.Data.Enums;

namespace MessagingQueueProcessor.Data.Models
{
    public class Message
    {
        public required Guid Id { get; set; }

        public required MessageType Type { get; set; }

        public required MessageStatus Status { get; set; }

        public required string Payload { get; set; }

        public int RetryCount { get; set; } = default!;

        public required DateTime CreatedAt { get; set; } = default!;
    }
}
