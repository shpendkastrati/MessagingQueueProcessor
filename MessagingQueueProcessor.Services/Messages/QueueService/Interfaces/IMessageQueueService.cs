using MessagingQueueProcessor.Data.Models;
using MessagingQueueProcessor.Services.Messages.QueueService.Dtos;

namespace MessagingQueueProcessor.Services.Messages.MessageQueueService.Interfaces
{
    public interface IMessageQueueService
    {
        CreateMessageDto Enqueue(CreateMessageDto createMessageDto);

        bool TryDequeue(out Message message);

        int Count { get; }
    }
}
