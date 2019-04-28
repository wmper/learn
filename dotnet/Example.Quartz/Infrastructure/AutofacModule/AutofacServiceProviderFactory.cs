using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.Quartz;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Specialized;

namespace Example.Quartz.Infrastructure.AutofacModule
{
    public class AutofacServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.Populate(services);
            builder.RegisterModule(new DefaultModule());
            builder.RegisterModule(new QuartzAutofacFactoryModule()
            {
                ConfigurationProvider = c =>
                {
                    var config = new NameValueCollection {
                            { "quartz.scheduler.instanceName", "Example.Quartz" },
                            { "quartz.threadPool.type", "Quartz.Simpl.SimpleThreadPool, Quartz" },
                            { "quartz.threadPool.threadCount", "10" },
                            { "quartz.jobStore.misfireThreshold", "60000" },
                            { "quartz.plugin.jobInitializer.type" ,"Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz.Plugins" },
                            { "quartz.plugin.jobInitializer.fileNames", "quartz_jobs.xml" },
                            { "quartz.plugin.jobInitializer.failOnFileNotFound", "true" },
                            { "quartz.plugin.jobInitializer.scanInterval" , "120" }
                        };

                    return config;
                }
            });
            builder.RegisterModule(new QuartzAutofacJobsModule(typeof(Program).Assembly));

            return builder;
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            var container = containerBuilder.Build();

            return new AutofacServiceProvider(container);
        }
    }
}
