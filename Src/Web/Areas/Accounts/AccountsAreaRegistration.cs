using System.Web.Mvc;
using System.Web.Routing;
using MvcAndBackbone.Areas.Accounts.Controllers;
using RestfulRouting;

namespace MvcAndBackbone.Areas.Accounts
{
    public class AccountsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Accounts";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapRoutes<AccountsAreaRouteSet>();
        }
    }

    public class AccountsAreaRouteSet : RouteSet
    {
        const string AreaName = "Accounts";

        public override void Map(IMapper map)
        {
            map.Area<UserSessionController>(AreaName, m => m.Resources<UserSessionController>());
        }
    }
}
