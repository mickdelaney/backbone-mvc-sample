using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dashboard.App.Repositories;
using Dashboard.Models;

namespace Dashboard.Controllers
{
    public class DashboardController : Controller
    {
        readonly LogMessagesRepository _logMessagesRepository;

        public DashboardController(LogMessagesRepository logMessagesRepository)
        {
            _logMessagesRepository = logMessagesRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var messages = _logMessagesRepository.GetMessages();
            var viewModel = new LogMessagesViewModel(messages);
            return View(viewModel);
        }
    }
}
