using Autofac;
using SDK.Extensions;

namespace Example.Quartz.Infrastructure.AutofacModule
{
    public class DefaultModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // 注册服务
            builder.RegisterAll("Example.Quartz");
        }
    }
}
