using Mapster;
using MessagingQueueProcessor.Common;
using MessagingQueueProcessor.Data.Models;
using MessagingQueueProcessor.Services.Messages.MessageQueueService.Interfaces;
using MessagingQueueProcessor.Services.Messages.QueueService.Dtos;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace MessagingQueueProcessor.Services.Messages.MessageQueueService
{
    public class MessageQueueService(ILogger<MessageQueueService> logger) : IMessageQueueService
    {
        private readonly ConcurrentQueue<Message> _queue = new();
        private readonly ILogger<MessageQueueService> _logger = Check.IsNotNull(logger);

        public int Count => _queue.Count;

        public CreateMessageDto Enqueue(CreateMessageDto createMessageDto)
        {
            Check.IsNotNull(createMessageDto, nameof(CreateMessageDto));

            // Improvements with more time
            // TODO: Implement FluentValidation rules using "RuleFor" to ensure the DTO meets input validation criteria.
            // TODO: Consider adding a separate service to perform business logic validation on the DTO.


            createMessageDto.Id = Guid.NewGuid();

            var message = createMessageDto.Adapt<Message>();

            _queue.Enqueue(message);

            _logger.LogInformation($"Enqueued message {message.Id} of type {message.Type}");

            return createMessageDto;
        }

        public bool TryDequeue(out Message message)
        {
            return _queue.TryDequeue(out message);
        }
    }
}
