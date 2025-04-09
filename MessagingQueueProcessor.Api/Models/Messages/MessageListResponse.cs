using System.ComponentModel.DataAnnotations;

namespace MessagingQueueProcessor.Api.Models.Messages
{
    public sealed record MessageListResponse
    {
        [Required]
        public List<MessageResponse> Data { get; init; }

        [Required]
        public long Count { get; init; }
    }
}
