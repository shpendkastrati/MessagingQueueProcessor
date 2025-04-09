using MessagingQueueProcessor.Data.Models;
using static MessagingQueueProcessor.Services.Common.Repositories.Interfaces.IGenericRepository;

namespace MessagingQueueProcessor.Services.Messages.Services.Repositories.Interfaces
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
    }
}
