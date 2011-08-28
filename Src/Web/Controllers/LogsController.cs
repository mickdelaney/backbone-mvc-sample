using System.Web.Mvc;

namespace MvcAndBackbone.Controllers
{
    public class LogsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

    }
}
