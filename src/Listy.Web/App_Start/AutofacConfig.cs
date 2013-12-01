using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Listy.Core.Configuration;
using Listy.Web.App_Start.nh;
using NServiceBus;

namespace Listy.Web.App_Start
{
    public static class AutofacConfig
    {
        public static IContainer Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<FileSystem>();

            var configurationProvider = new ListyWebConfigurationProvider();

            builder
                .RegisterInstance(configurationProvider)
                .SingleInstance()
                ;

            var sessionFactory = NhibernateConfig.Register(configurationProvider);
            builder
                .RegisterInstance(sessionFactory)
                .AsImplementedInterfaces()
                ;

            var assemblies = new[]
                {
                    typeof (MvcApplication).Assembly,
                    typeof (IConfigurationProvider).Assembly,
                    //typeof(..something in Listy.Data).Assembly,
                };

            builder.RegisterAssemblyTypes(assemblies);
            builder.RegisterAssemblyModules(assemblies);

            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterFilterProvider();
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            builder
                .RegisterControllers(assemblies)
                .OnActivated(e =>
                    {
                        dynamic viewBag = ((Controller) e.Instance).ViewBag;
                        // add things to ViewBag
                    });

    //        startableBus = Configure.With()
    //.DefaultBuilder()
    //.Log4Net(new DebugAppender { Threshold = Level.Warn })
    //.UseTransport<Msmq>()
    //.PurgeOnStartup(true)
    //.UnicastBus()
    //.RunHandlersUnderIncomingPrincipal(false)
    //.RijndaelEncryptionService()
    //.CreateBus();

            var bus = Configure.With()
                               .DefaultBuilder()
                               .CreateBus();
            builder.RegisterInstance(bus).AsImplementedInterfaces();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            return container;
        }
    }
}