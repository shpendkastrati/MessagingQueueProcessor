using MessagingQueueProcessor.Api.Models.Metrics;
using MessagingQueueProcessor.Common;
using MessagingQueueProcessor.Services.Messages.MessageQueueService.Interfaces;
using MessagingQueueProcessor.Services.Metrics.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MessagingQueueProcessor.Api.Controllers
{
    public class DashboardController(
        IMetricsService metricsService,
        IMessageQueueService queueService) : Controller
    {
        private readonly IMetricsService _metricsService = Check.IsNotNull(metricsService);
        private readonly IMessageQueueService _queueService = Check.IsNotNull(queueService);

        /// <summary>
        /// https://localhost:7107/Dashboard
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                ProcessedMessages = _metricsService.ProcessedCount,
                ErrorCount = _metricsService.ErrorCount,
                QueueDepth = _queueService.Count
            };

            return View(model);
        }
    }
}
