using Mapster;
using MessagingQueueProcessor.Common;
using MessagingQueueProcessor.Common.Exceptions;
using MessagingQueueProcessor.Data.Models;
using MessagingQueueProcessor.Services.Messages.Services.Dtos;
using MessagingQueueProcessor.Services.Messages.Services.Interfaces;
using MessagingQueueProcessor.Services.Messages.Services.Repositories.Interfaces;
using System.Linq.Expressions;

namespace MessagingQueueProcessor.Services.Messages.Services
{
    public class MessageService(IMessageRepository messageRepository) : IMessageService
    {
        private readonly IMessageRepository _messageRepository = Check.IsNotNull(messageRepository);

        public async Task<MessageDto> GetAsync(Guid id)
        {
            Check.IsNotNull(id, nameof(id));

            var message = await messageRepository.GetAsync<Message>(m => m.Id == id)
                ?? throw new NotFoundException($"Message with Id {id} not found!");

            return message.Adapt<MessageDto>();
        }

        public async Task<IReadOnlyList<MessageItemDto>> GetListAsync(MessageFilterDto messageFilterDto)
        {
            Check.IsNotNull(messageFilterDto, nameof(messageFilterDto));

            var filter = CreateFilter(messageFilterDto);

            return await _messageRepository.GetListWithFilterAsync<MessageItemDto>(filter);
        }

        private static Expression<Func<Message, bool>> CreateFilter(MessageFilterDto filter)
        {
            var predicate = PredicateBuilder.True<Message>();

            if (!string.IsNullOrEmpty(filter.Search))
            {
                predicate = predicate.And(x => x.Id.ToString().Contains(filter.Search));
            }

            return predicate;
        }
    }
}
