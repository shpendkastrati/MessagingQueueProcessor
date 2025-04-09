using Mapster;
using MapsterMapper;
using MessagingQueueProcessor.Api.Mapster;

namespace MessagingQueueProcessor.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add mapster
        /// </summary>
        /// <param name="services"></param>
        /// <returns>IServiceCollection so that additional calls can be chained.</returns>
        public static IServiceCollection AddMapster(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;

            ConfigureMappings.ConfigureMapster(config);

            Services.Mapster.ConfigureMappings.ConfigureMapster(config);

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}
