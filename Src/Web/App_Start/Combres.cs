[assembly: WebActivator.PreApplicationStartMethod(typeof(MvcAndBackbone.App_Start.Combres), "PreStart")]
namespace MvcAndBackbone.App_Start {
	using System.Web.Routing;
	using global::Combres;
	
    public static class Combres {
        public static void PreStart() {
            RouteTable.Routes.AddCombresRoute("Combres");
        }
    }
}