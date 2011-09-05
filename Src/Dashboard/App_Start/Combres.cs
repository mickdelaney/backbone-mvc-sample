[assembly: WebActivator.PreApplicationStartMethod(typeof(Dashboard.App_Start.Combres), "PreStart")]
namespace Dashboard.App_Start {
	using System.Web.Routing;
	using global::Combres;
	
    public static class Combres {
        public static void PreStart() {
            RouteTable.Routes.AddCombresRoute("Combres");
        }
    }
}