using Core;
using MvcAndBackbone.App.Repositories;
using StructureMap.Configuration.DSL;

namespace MvcAndBackbone.Core
{
    public class WebAppRegistry : Registry
    {
        public WebAppRegistry()
        {
            For<UserSessionRepository>();
            For<RabbitMqService>().Singleton()
                                  .Use<RabbitMqService>()
                                  .Ctor<string>().Is("localhost");
        }
    }
}