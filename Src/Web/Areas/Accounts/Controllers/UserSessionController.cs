using System;
using System.Linq;
using System.Web.Mvc;
using MvcAndBackbone.Areas.Accounts.Models;

namespace MvcAndBackbone.Areas.Accounts.Controllers
{
    public class UserSessionController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var sessions = MvcApplication.UserSession.ToList();
            if(Request.IsAjaxRequest())
            {
                return Json(sessions, JsonRequestBehavior.AllowGet);
            }

            return View(sessions);
        }

        [HttpGet]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Update(UserSession userSession)
        {
            if (!ModelState.IsValid)
            {
                return Json(ModelState.Values);
            }

            var session = MvcApplication.UserSession
                                        .Where(u => u.Id == userSession.Id)
                                        .FirstOrDefault();

            session.Name = userSession.Name;
            session.Tags.Clear();
            session.Tags.AddRange(userSession.Tags);

            if (Request.IsAjaxRequest())
            {
                return Json(session, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(UserSession userSession)
        {
            if(!ModelState.IsValid)
            {
                return Json(ModelState.Values);
            }

            userSession.Id = Guid.NewGuid();

            userSession.Tags.Add("Test 1");
            userSession.Tags.Add("Test 2");
            userSession.Tags.Add("Test 3");
            userSession.Tags.Add("Test 4");

            MvcApplication.UserSession.Add(userSession);

            if(Request.IsAjaxRequest())
            {
                return Json(userSession, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var userSession = MvcApplication.UserSession
                                            .Where(u => u.Id == id)
                                            .FirstOrDefault();

            if (Request.IsAjaxRequest())
            {
                return Json(userSession, JsonRequestBehavior.AllowGet);
            }

            return View(userSession);
        }
    }
}
