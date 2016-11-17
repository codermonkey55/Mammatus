using Autofac;
using Mammatus.Integration.Wcf.Autofac;
using Mammatus.ServiceModel.Extensibility.DependencyInjection.Providers;
using Mammatus.ServiceModel.IntegrationTest.Services;
using System.Configuration;

namespace Mammatus.ServiceModel.IntegrationTest.Bootstrapper
{
    internal static class TestBootstrapper
    {
        internal static void Configure()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<TestService>().AsSelf();

            IContainer container = builder.Build();

            ServiceResolverProvider.InitProvider(() => new AutofacServiceResolver(container));

            var fileLoggerIdentifier = ConfigurationManager.AppSettings["FileErrorLogger.Configuration.Name"];
            var eventLoggerIdentifier = ConfigurationManager.AppSettings["EventErrorLogger.Configuration.Name"];

            LoggingInitializer.Initialize(eventLoggerIdentifier: eventLoggerIdentifier,
                                          fileLoggerIdentifier: fileLoggerIdentifier,
                                          serviceName: "TestService");
        }
    }
}
