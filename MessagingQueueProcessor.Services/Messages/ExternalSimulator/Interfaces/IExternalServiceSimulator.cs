using MessagingQueueProcessor.Data.Models;

namespace MessagingQueueProcessor.Services.Messages.ExternalSimulator.Interfaces
{
    public interface IExternalServiceSimulator
    {
        Task SendMessageAsync(Message message, CancellationToken cancellationToken);
    }
}
