using System.ComponentModel.DataAnnotations;

namespace MessagingQueueProcessor.Api.Models.Common
{
    public sealed record EntityCreatedResponse
    {
        [Required]
        public string Id { get; init; } = null!;
    }
}
