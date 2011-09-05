using Core;
using StructureMap.Configuration.DSL;

namespace Dashboard.Core
{
    public class WebAppRegistry : Registry
    {
        public WebAppRegistry()
        {
            For<RabbitMqService>().Singleton()
                                  .Use<RabbitMqService>()
                                  .Ctor<string>().Is("localhost");
        }
    }
}