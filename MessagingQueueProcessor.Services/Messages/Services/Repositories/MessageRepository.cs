using MessagingQueueProcessor.Data;
using MessagingQueueProcessor.Data.Models;
using MessagingQueueProcessor.Services.Common.Repositories;
using MessagingQueueProcessor.Services.Messages.Services.Repositories.Interfaces;

namespace MessagingQueueProcessor.Services.Messages.Services.Repositories
{
    public class MessageRepository(ApplicationDbContext context) : GenericRepository<Message>(context), IMessageRepository
    {
    }
}
