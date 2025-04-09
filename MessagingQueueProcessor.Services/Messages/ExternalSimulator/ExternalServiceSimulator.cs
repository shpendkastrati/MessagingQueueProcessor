using MessagingQueueProcessor.Common;
using MessagingQueueProcessor.Data.Models;
using MessagingQueueProcessor.Services.Messages.ExternalSimulator.Interfaces;
using Microsoft.Extensions.Logging;

namespace MessagingQueueProcessor.Services.Messages.ExternalSimulator
{
    public class ExternalServiceSimulator(ILogger<ExternalServiceSimulator> logger) : IExternalServiceSimulator
    {
        private readonly ILogger<ExternalServiceSimulator> _logger = Check.IsNotNull(logger);
        private readonly Random _random = new Random();

        public async Task SendMessageAsync(Message message, CancellationToken cancellationToken)
        {
            // To do:
            // Simulate delay between 0.5s and 1s.
            await Task.Delay(_random.Next(500, 1000), cancellationToken);

            // To do:
            // Simulate a 30% chance of failure.
            if (_random.NextDouble() < 0.3)
            {
                throw new Exception("Simulated external API failure");
            }

            _logger.LogInformation($"Message {message.Id} sent successfully via {message.Type}.");
        }
    }
}
