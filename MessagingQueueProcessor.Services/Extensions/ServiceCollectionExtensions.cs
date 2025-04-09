using MessagingQueueProcessor.Services.Common.Repositories;
using MessagingQueueProcessor.Services.Messages.ExternalSimulator;
using MessagingQueueProcessor.Services.Messages.ExternalSimulator.Interfaces;
using MessagingQueueProcessor.Services.Messages.MessageQueueService;
using MessagingQueueProcessor.Services.Messages.MessageQueueService.Interfaces;
using MessagingQueueProcessor.Services.Messages.Services;
using MessagingQueueProcessor.Services.Messages.Services.Interfaces;
using MessagingQueueProcessor.Services.Messages.Services.Repositories;
using MessagingQueueProcessor.Services.Messages.Services.Repositories.Interfaces;
using MessagingQueueProcessor.Services.Metrics.Services;
using MessagingQueueProcessor.Services.Metrics.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using static MessagingQueueProcessor.Services.Common.Repositories.Interfaces.IGenericRepository;

namespace MessagingQueueProcessor.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterMessagingQueueServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Messages
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddSingleton<IMessageQueueService, MessageQueueService>();
            services.AddTransient<IExternalServiceSimulator, ExternalServiceSimulator>();

            // Metrics
            services.AddSingleton<IMetricsService, MetricsService>();

            return services;
        }
    }
}
