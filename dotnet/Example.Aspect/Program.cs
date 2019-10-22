using AspectCore.DynamicProxy;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Example.Aspect
{
    public interface IMall
    {
        void Write();
    }

    public class MallService : IMall
    {
        [UseCache]
        public void Write()
        {
            Console.WriteLine("mall");
            throw new Exception("test");
        }
    }

    public class UseCacheAttribute : AbstractInterceptorAttribute
    {
        //[FromContainer]
        //private ICache _cache { get; set; }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            //var _cache = context.ServiceProvider.GetService<ICache>();
            // TODO Cache Get

            // 获取方法参数数组
            object[] paramters = context.Parameters;

            Console.WriteLine("执行前:");


            try
            {
                await next.Invoke(context);
            }
            catch (Exception)
            {
                Console.WriteLine("exception.");
            }


            // 方法返回值内容
            var obj = context.ReturnValue;
            if (obj != null)
            {
                // TODO Cache Set
            }

            Console.WriteLine("执行后:");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var mall = new ServiceCollection().AddTransient<IMall, MallService>()
                                              .BuildAspectInjectorProvider()
                                              .GetService<IMall>();
            mall.Write();

            Console.WriteLine("the end.");
            Console.Read();
        }
    }
}
