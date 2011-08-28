using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcAndBackbone.Areas.Accounts.Models;
using MvcAndBackbone.Controllers;
using RestfulRouting;
using RestfulRouting.Mappers;
using StructureMap;

namespace MvcAndBackbone
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IContainer Container;

        public static IList<UserSession> UserSession = new List<UserSession>();

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RouteTable.Routes.MapRoutes<DefaultRoutes>();

            //routes.MapRoute(
            //    "Default", // Route name
            //    "{controller}/{action}/{id}", // URL with parameters
            //    new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            //);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //ViewEngines.Engines.Clear();
            //ViewEngines.Engines.Add(new RestfulRoutingRazorViewEngine());

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ConfigureContainer();
        }

        public void ConfigureContainer()
        {
            Container = new Container();
            Container.Configure(x => x.Scan(scan => {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            }));
        }
    }

    public class DefaultRoutes : RouteSet
    {
        public override void Map(IMapper map)
        {
            map.Root<LogsController>(r => r.Index());
            map.Resources<LogsController>();
        }
    }
}