using MessagingQueueProcessor.Common;
using MessagingQueueProcessor.Data.Enums;
using MessagingQueueProcessor.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MessagingQueueProcessor.Services.Messages.Services.Repositories.Interfaces;
using MessagingQueueProcessor.Services.Messages.ExternalSimulator.Interfaces;
using MessagingQueueProcessor.Services.Messages.MessageQueueService.Interfaces;

namespace MessagingQueueProcessor.Services.Messages.ProcessorService
{
    public class MessageProcessorService(
        ILogger<MessageProcessorService> logger,
        IMessageQueueService queueService,
        IServiceScopeFactory scopeFactory,
        IExternalServiceSimulator externalService,
        IConfiguration configuration,
        IOptions<ProcessingOptions> processingOptions) : BackgroundService
    {
        private readonly ILogger<MessageProcessorService> _logger = Check.IsNotNull(logger);
        private readonly IMessageQueueService _queueService = Check.IsNotNull(queueService);
        private readonly IServiceScopeFactory _scopeFactory = Check.IsNotNull(scopeFactory);
        private readonly IExternalServiceSimulator _externalService = Check.IsNotNull(externalService);
        private readonly IConfiguration _configuration = Check.IsNotNull(configuration);
        private readonly ProcessingOptions _processingOptions = processingOptions.Value;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Message Processor Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_queueService.TryDequeue(out Message message))
                {
                    _ = ProcessMessageAsync(message, stoppingToken);
                }
                else
                {
                    await Task.Delay(500, stoppingToken);
                }
            }

            _logger.LogInformation("Message Processor Service is stopping.");
        }

        private async Task ProcessMessageAsync(Message message, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Started processing message: {message.Id}");

            message.Status = MessageStatus.Processing;

            // Create a DI scope for EF Core usage.
            using (var scope = _scopeFactory.CreateScope())
            {
                var scopedMessageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();
                bool processed = false;
                while (message.RetryCount < 3 && !processed)
                {
                    try
                    {
                        await _externalService.SendMessageAsync(message, cancellationToken);
                        message.Status = MessageStatus.Sent;
                        processed = true;
                    }
                    catch (Exception ex)
                    {
                        message.RetryCount++;

                        _logger.LogWarning(ex, $"Processing failed for message {message.Id}. Retry attempt: {message.RetryCount}");

                        if (message.RetryCount >= 3)
                        {
                            message.Status = MessageStatus.DeadLetter;
                        }
                        else
                        {
                            int delay = 1000 * (int)Math.Pow(2, message.RetryCount);
                            await Task.Delay(delay, cancellationToken);
                        }
                    }
                }

                scopedMessageRepository.Add(message);
                await scopedMessageRepository.SaveChangesAsync();
            }

            await Task.Delay(_processingOptions.ThrottleDelay, cancellationToken);
            _logger.LogInformation($"Finished processing message: {message.Id}");
        }
    }
}
