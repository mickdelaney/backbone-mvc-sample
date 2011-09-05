using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Core;
using Dashboard.App.Repositories;
using Dashboard.App.Services;
using Dashboard.Controllers;
using RestfulRouting;
using StructureMap;

namespace Dashboard
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IContainer Container;
        public static LogMessagesRepository MessagesRepository = new LogMessagesRepository();
        
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RouteTable.Routes.MapRoutes<DefaultRoutes>();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ConfigureContainer();

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(Container));


            var listener = Container.GetInstance<LogMessagesListener>();
            var task = listener.Init();
            task.ContinueWith(t => Console.WriteLine("Finished handling message"));
        }
        
        public void ConfigureContainer()
        {
            Container = new Container();
            Container.Configure(x => x.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
                scan.LookForRegistries();
            }));

            Container.Configure(c =>
            {
                c.For<LogMessagesRepository>().Singleton().Use(MessagesRepository);
                c.For<LogMessagesListener>().Singleton();
            });
        }
    }

    public class DefaultRoutes : RouteSet
    {
        public override void Map(IMapper map)
        {
            map.Root<DashboardController>(r => r.Index());
        }
    }
}