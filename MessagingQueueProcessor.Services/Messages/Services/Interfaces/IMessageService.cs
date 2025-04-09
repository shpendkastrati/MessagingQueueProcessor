using MessagingQueueProcessor.Services.Messages.Services.Dtos;

namespace MessagingQueueProcessor.Services.Messages.Services.Interfaces
{
    public interface IMessageService
    {
        Task<MessageDto> GetAsync(Guid id);

        Task<IReadOnlyList<MessageItemDto>> GetListAsync(MessageFilterDto messageFilterDto);
    }
}
