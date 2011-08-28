using MvcAndBackbone.App.Repositories;
using StructureMap.Configuration.DSL;

namespace MvcAndBackbone.Core
{
    public class WebAppRegistry : Registry
    {
        public WebAppRegistry()
        {
            For<UserSessionRepository>();
        }
    }
}