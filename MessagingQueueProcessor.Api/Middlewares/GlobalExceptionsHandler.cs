using MessagingQueueProcessor.Api.Models.Common;
using MessagingQueueProcessor.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace MessagingQueueProcessor.Api.Middlewares
{
    internal static class GlobalExceptionsHandler
    {
        private static readonly JsonSerializerOptions _serializeOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        internal static Action<IApplicationBuilder> HandleGlobalExceptions(WebApplication app)
        {
            return a => a.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = feature?.Error!;

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                if (exception is BusinessValidationException businessValidationException)
                {
                    context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                    var response = JsonSerializer.Serialize(
                        new ValidationErrorsResponse
                        {
                            Messages = businessValidationException.Messages,
                        },
                        _serializeOptions);
                    await context.Response.WriteAsync(response);
                }
                else if (exception is NotFoundException notFoundException)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync(string.Empty);
                }
                else
                {
                    using var scope = app.Services.CreateScope();
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exception, "Ooops, error occured!");

                    var response = JsonSerializer.Serialize(
                        new
                        {
                            message = exception.Message,
                        },
                        _serializeOptions);
                    await context.Response.WriteAsync(response);
                }
            });
        }
    }
}
