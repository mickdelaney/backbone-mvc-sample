using System.Text;
using System.Web.Mvc;
using Core;
using RabbitMQ.Client.Framing.v0_9;

namespace MvcAndBackbone.Controllers
{
    public class LogsController : Controller
    {
        readonly RabbitMqService _rabbitMqService;

        public LogsController(RabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string message)
        {
            var channel = _rabbitMqService.CreateModel(AppConstants.ExchangeName, AppConstants.QueueName, AppConstants.RoutingKey);
            
            var messageBody = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(AppConstants.ExchangeName, AppConstants.RoutingKey, null, messageBody);
            channel.Close(Constants.ReplySuccess, "Closing the channel");

            return RedirectToAction("Index");
        }
    }
}
