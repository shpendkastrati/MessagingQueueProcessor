using FluentAssertions;
using MessagingQueueProcessor.Api.Controllers;
using MessagingQueueProcessor.Api.Models.Common;
using MessagingQueueProcessor.Api.Models.Messages;
using MessagingQueueProcessor.Data.Enums;
using MessagingQueueProcessor.Services.Messages.MessageQueueService.Interfaces;
using MessagingQueueProcessor.Services.Messages.QueueService.Dtos;
using MessagingQueueProcessor.Services.Messages.Services.Dtos;
using MessagingQueueProcessor.Services.Messages.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace MessagingQueueProcessor.Api.Test
{
    public class MessagesControllerTest
    {
        [Fact]
        public void ShouldCreateMessageAndReturnCreatedResult()
        {
            // Arrange
            Guid messageId = new();
            var createMessageRequest = new CreateMessageRequest
            {
                Type = MessageType.PushNotification,
                Status = MessageStatus.Sent,
                Payload = "Test",
                RetryCount = 1,
            };
            var returnDto = new CreateMessageDto
            {
                Id = messageId,
                Type = MessageType.PushNotification,
                Status = MessageStatus.Sent,
                Payload = "Test",
                RetryCount = 1,
            };

            var messageQueueServiceSub = Substitute.For<IMessageQueueService>();
            messageQueueServiceSub.Enqueue(Arg.Any<CreateMessageDto>()).Returns(returnDto);

            var controller = new MessagesController();

            // Act
            var result = controller.CreateMessage(createMessageRequest, messageQueueServiceSub);

            // Assert
            var createdResult = result.Should().BeOfType<AcceptedResult>().Subject;
            createdResult.Should().NotBeNull();

            var entityCreatedResponse = createdResult.Value.Should().BeAssignableTo<EntityCreatedResponse>().Subject;
            entityCreatedResponse.Should().NotBeNull();
            entityCreatedResponse.Id.Should().Be(messageId.ToString());

            messageQueueServiceSub.Received(1).Enqueue(Arg.Any<CreateMessageDto>());
        }

        [Fact]
        public async Task ShouldGetMessageStatusAndReturnsOkResultWithData()
        {
            // Arrange
            Guid messageId = new();
            var messageDto = new MessageDto
            {
                Id = messageId,
                Type = MessageType.PushNotification,
                Status = MessageStatus.Sent,
                Payload = "Test",
                RetryCount = 1,
            };

            var messageServiceSub = Substitute.For<IMessageService>();
            messageServiceSub.GetAsync(Arg.Any<Guid>()).Returns(messageDto);

            var controller = new MessagesController();

            // Act
            var result = await controller.GetMessageStatus(messageId, messageServiceSub);

            // Assert
            var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var messagesResponse = okObjectResult.Value.Should().BeAssignableTo<MessageResponse>().Subject;

            messagesResponse.Should().BeEquivalentTo(new MessageResponse
            {
                Id = messageId,
                Type = MessageType.PushNotification,
                Status = MessageStatus.Sent,
                Payload = "Test",
                RetryCount = 1,
            });

            await messageServiceSub.Received(1).GetAsync(messageId);
        }

        [Fact]
        public async Task ShouldGetStatisticsAndReturnsOkResultWithData()
        {
            // Arrange
            var messageServiceSub = Substitute.For<IMessageService>();

            var messageList = new List<MessageItemDto>
            {
                new MessageItemDto
                {
                    Id = Guid.NewGuid(),
                    Type = MessageType.PushNotification,
                    Status = MessageStatus.Sent,
                    Payload = "Test 1",
                    RetryCount= 1,
                },
                new MessageItemDto
                {
                    Id = Guid.NewGuid(),
                    Type = MessageType.PushNotification,
                    Status = MessageStatus.Sent,
                    Payload = "Test 2",
                    RetryCount= 1,
                }
            };

            messageServiceSub.GetListAsync(Arg.Any<MessageFilterDto>()).Returns(messageList);

            var controller = new MessagesController();

            // Act
            var result = await controller.GetStatistics(messageServiceSub);

            // Assert
            var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var statisticResponse = okObjectResult.Value.Should().BeAssignableTo<IEnumerable<object>>().Subject;

            statisticResponse.Should().NotBeNull();
            statisticResponse.Should().HaveCount(1);

            statisticResponse.Should().BeEquivalentTo(new[]
            {
                new
                {
                    Type = MessageType.PushNotification,
                    Status = MessageStatus.Sent,
                    Count = 2
                }
            });

            await messageServiceSub.Received(1).GetListAsync(Arg.Any<MessageFilterDto>());
        }

        [Fact]
        public async Task ShouldReturnNoContentWhenNoStatisticsExist()
        {
            // Arrange
            var messageServiceSub = Substitute.For<IMessageService>();
            messageServiceSub.GetListAsync(Arg.Any<MessageFilterDto>()).Returns(new List<MessageItemDto>());

            var controller = new MessagesController();

            // Act
            var result = await controller.GetStatistics(messageServiceSub);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            await messageServiceSub.Received(1).GetListAsync(Arg.Any<MessageFilterDto>());
        }

        [Fact]
        public async Task ShouldGetMessagesAndReturnOkWithMessageList()
        {
            // Arrange
            var messageServiceSub = Substitute.For<IMessageService>();

            var messageList = new List<MessageItemDto>
            {
                new MessageItemDto
                {
                    Id = Guid.NewGuid(),
                    Type = MessageType.Email,
                    Status = MessageStatus.Sent,
                    Payload = "Email content",
                    RetryCount = 0
                },
                new MessageItemDto
                {
                    Id = Guid.NewGuid(),
                    Type = MessageType.PushNotification,
                    Status = MessageStatus.Pending,
                    Payload = "Push content",
                    RetryCount = 1
                }
            };

            messageServiceSub.GetListAsync(Arg.Any<MessageFilterDto>()).Returns(messageList);

            var controller = new MessagesController();

            // Act
            var result = await controller.GetMessages(messageServiceSub);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeAssignableTo<MessageListResponse>().Subject;

            response.Should().NotBeNull();
            response.Data.Should().HaveCount(2);
            response.Count.Should().Be(2);

            await messageServiceSub.Received(1).GetListAsync(Arg.Any<MessageFilterDto>());
        }

        [Fact]
        public async Task ShouldReturnNoContentWhenNoMessagesExist()
        {
            // Arrange
            var messageServiceSub = Substitute.For<IMessageService>();
            messageServiceSub.GetListAsync(Arg.Any<MessageFilterDto>()).Returns(new List<MessageItemDto>());

            var controller = new MessagesController();

            // Act
            var result = await controller.GetMessages(messageServiceSub);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            await messageServiceSub.Received(1).GetListAsync(Arg.Any<MessageFilterDto>());
        }
    }
}
