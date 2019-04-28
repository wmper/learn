using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.Quartz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Specialized;

namespace Example.Quartz.Infrastructure.AutofacModule
{
    public class AutofacServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var properties = ConfigurationManager.Configuration.GetSection("quartz").Get<Properties>();

            var config = new NameValueCollection {
                            { "quartz.scheduler.instanceName", properties.Scheduler.InstanceName },
                            { "quartz.threadPool.type", properties.ThreadPool.Type },
                            { "quartz.threadPool.threadCount", properties.ThreadPool.ThreadCount.ToString() },
                            { "quartz.jobStore.misfireThreshold", properties.JobStore.MisfireThreshold.ToString() },
                            { "quartz.plugin.jobInitializer.type" ,properties.Plugin.JobInitializer.Type},
                            { "quartz.plugin.jobInitializer.fileNames", properties.Plugin.JobInitializer.FileNames},
                            { "quartz.plugin.jobInitializer.failOnFileNotFound", properties.Plugin.JobInitializer.FailOnFileNotFound.ToString() },
                            { "quartz.plugin.jobInitializer.scanInterval" , properties.Plugin.JobInitializer.ScanInterval.ToString()}
                        };
            var builder = new ContainerBuilder();

            builder.Populate(services);
            builder.RegisterModule(new DefaultModule());
            builder.RegisterModule(new QuartzAutofacFactoryModule()
            {
                ConfigurationProvider = c =>
                {
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
