using Mapster;
using MessagingQueueProcessor.Api.Models.Common;
using MessagingQueueProcessor.Api.Models.Messages;
using MessagingQueueProcessor.Services.Messages.MessageQueueService.Interfaces;
using MessagingQueueProcessor.Services.Messages.QueueService.Dtos;
using MessagingQueueProcessor.Services.Messages.Services.Dtos;
using MessagingQueueProcessor.Services.Messages.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MessagingQueueProcessor.Api.Controllers
{
    [ApiController]
    [Route("messagingprocessor/v1/messages")]
    public class MessagesController : ControllerBase
    {
        /// <summary>
        /// Create a message
        /// </summary>
        /// <param name="model">CreateMessageRequest</param>
        /// <returns>EntityCreatedResponse</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Create a message")]
        [SwaggerResponse(StatusCodes.Status202Accepted, "Request Accepted", typeof(EntityCreatedResponse))]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Validation errors in the request body or query params", typeof(ValidationErrorsResponse))]
        public IActionResult CreateMessage(
            CreateMessageRequest model,
            [FromServices] IMessageQueueService messageQueueService)
        {
            var message = messageQueueService.Enqueue(model.Adapt<CreateMessageDto>());

            return Accepted(new EntityCreatedResponse() { Id = message.Id.ToString() });
        }

        /// <summary>
        /// Get message status
        /// </summary>
        /// <param name="id">The Id of the message</param>
        /// <returns>MessageResponse</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get message status")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(MessageResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Message has not been found")]
        public async Task<IActionResult> GetMessageStatus(Guid id, [FromServices] IMessageService messageService)
        {
            var message = await messageService.GetAsync(id);

            return new OkObjectResult(message.Adapt<MessageResponse>());
        }

        /// <summary>
        /// Get message statistics
        /// </summary>
        /// <returns></returns>
        [HttpGet("messages")]
        [SwaggerOperation(Summary = "Get message statistics")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "There are no message statistics found.")]
        public async Task<IActionResult> GetStatistics([FromServices] IMessageService messageService)
        {
            var statistics = await messageService.GetListAsync(new MessageFilterDto());

            if (!statistics.Any())
                return NoContent();

            var groupedStatistics = statistics.GroupBy(m => new { m.Type, m.Status })
                    .Select(g => new
                    {
                        g.Key.Type,
                        g.Key.Status,
                        Count = g.Count()
                    }).ToList();

            return Ok(groupedStatistics);
        }

        /// <summary>
        /// Get messages list
        /// </summary>
        /// <returns>MessageListResponse</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Get messages list")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(MessageListResponse))]
        [SwaggerResponse(StatusCodes.Status204NoContent, "There are no messages found.")]
        public async Task<IActionResult> GetMessages([FromServices] IMessageService messageService)
        {
            var messages = await messageService.GetListAsync(new MessageFilterDto());

            return !messages.Any() ? NoContent()
               : Ok(new MessageListResponse
               {
                   Data = messages.Adapt<List<MessageResponse>>(),
                   Count = messages.Count,
               });
        }
    }
}
