using MessagingQueueProcessor.Api.Extensions;
using MessagingQueueProcessor.Api.Middlewares;
using MessagingQueueProcessor.Api.Swagger;
using MessagingQueueProcessor.Common;
using MessagingQueueProcessor.Data;
using MessagingQueueProcessor.Services.Extensions;
using MessagingQueueProcessor.Services.Messages.ProcessorService;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SQLitePCL;

Batteries.Init();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlite($"Data Source={Path.Combine(AppContext.BaseDirectory, "messages.db")}"));

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

builder.Services.AddControllers();

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration.
    Enrich.
    FromLogContext().
    WriteTo.Console();
});


builder.Services.AddSwaggerGen(SwaggerGen.ConfigureSwagger());

var processingOptions = builder.Configuration.GetSection("ProcessingOptions");
builder.Services.Configure<ProcessingOptions>(processingOptions);

builder.Services.RegisterMessagingQueueServices();

// Register the background message processor
builder.Services.AddHostedService<MessageProcessorService>();

builder.Services.AddMapster();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(GlobalExceptionsHandler.HandleGlobalExceptions(app));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
