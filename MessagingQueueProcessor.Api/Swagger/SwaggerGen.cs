using System.Text.Json.Nodes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MessagingQueueProcessor.Api.Swagger
{
    public static class SwaggerGen
    {
        internal static Action<SwaggerGenOptions> ConfigureSwagger()
        {
            return c =>
            {
                c.DescribeAllParametersInCamelCase();
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MessagingQueueProcessor API", Version = "v1" });
                c.MapType<JsonObject>(() => new OpenApiSchema { Type = "object" });
                c.MapType<DateOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "date",
                    Description = "Date only. Input should be in ISO date format YYYY-MM-DD",
                });
            };
        }
    }
}
