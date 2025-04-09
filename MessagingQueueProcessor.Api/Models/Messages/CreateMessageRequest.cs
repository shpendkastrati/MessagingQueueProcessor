﻿using MessagingQueueProcessor.Data.Enums;

namespace MessagingQueueProcessor.Api.Models.Messages
{
    public sealed record CreateMessageRequest
    {
        public required MessageType Type { get; init; }

        public required MessageStatus Status { get; init; }

        public required string Payload { get; init; }

        public required int RetryCount { get; init; }
    }
}
