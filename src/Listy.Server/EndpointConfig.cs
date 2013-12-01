
using System.IO.Abstractions;
using Autofac;

namespace Listy.Server
{
    using NServiceBus;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
    {
	    public void Init()
	    {
            var builder = new ContainerBuilder();

            builder.RegisterType<FileSystem>();

            var sessionFactory =
                NhibernateConfig.Register(
                    "Data Source=(local);Initial Catalog=Listy;Trusted_Connection=True;MultipleActiveResultSets=True");
            builder
                .RegisterInstance(sessionFactory)
                .AsImplementedInterfaces()
                ;

            var assemblies = new[]
                {
                    typeof (EndpointConfig).Assembly,
                    //typeof(..something in Listy.Data).Assembly,
                };

            builder.RegisterAssemblyTypes(assemblies);
            builder.RegisterAssemblyModules(assemblies);

            Configure
                .With().AutofacBuilder(builder.Build())
                ;
	    }
    }
}
