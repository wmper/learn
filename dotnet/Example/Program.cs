using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Microsoft.Extensions.DependencyInjection;

namespace Example
{
    public interface IMall
    {
        void Write();
    }

    public class MallService : IMall
    {
        public void Write()
        {
            Console.WriteLine("mall");
        }
    }
    public interface IUser
    {
        IList<YieldTest> YieldTests(string id, IList<YieldTest> list);
    }

    public class UserService : IUser
    {
        [UseCache]
        public IList<YieldTest> YieldTests(string id, IList<YieldTest> ls)
        {
            var list = new List<YieldTest> {
                new YieldTest () { Name = "hello" }
            };

            Console.WriteLine("业务");

            return list;
        }
    }

    enum Day : byte { Sat = 1, Sun, Mon, Tue, Wed, Thu, Fri }

    public class UseCacheAttribute : AbstractInterceptorAttribute
    {
        [FromContainer]
        private IMall _mall { get; set; }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            //var mall = context.ServiceProvider.GetService<IMall>();
            _mall.Write();

            object[] paramters = context.Parameters;

            Console.WriteLine("1:" + paramters[0].ToString());

            await next.Invoke(context);

            var obj = (IList<YieldTest>)context.ReturnValue;

            Console.WriteLine("2:" + obj.FirstOrDefault().Name);
        }
    }

    public class YieldTest
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }

        public IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Age;
            yield return Province;
            yield return City;
            yield return Country;
            yield return Address;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            #region
            //Console.WriteLine("thread-1:" + Thread.CurrentThread.ManagedThreadId);

            //var id = "1";

            //var user = new ServiceCollection().AddTransient<IUser, UserService>()
            //                                  .AddTransient<IMall, MallService>()
            //                                  .BuildAspectInjectorProvider()
            //                                  .GetService<IUser>();

            //var list = user.YieldTests(id, null);

            //foreach (var item in list)
            //{
            //    Console.WriteLine(item.Name);
            //}

            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            //for (int i = 0; i < 20; i++)
            //{
            //    Test2();
            //    i++;
            //}

            //await TestAsync(0);

            //stopwatch.Stop();

            //Console.WriteLine("time:" + stopwatch.ElapsedMilliseconds + "ms");

            //var model = new YieldTest()
            //{
            //    Name = "小明",
            //    Age = 18,
            //    Province = "广东",
            //    City = "广州",
            //    Country = "天河",
            //    Address = "银汇大厦"
            //};

            //foreach (var item in model.GetAtomicValues())
            //{
            //    Console.WriteLine(item);
            //}

            //string str = "adfdafdfasdfaf";

            //Console.WriteLine(str[2]);

            //var w16 = 0x2A;
            //var w2 = 0b_0010_1010;

            //Console.WriteLine(w16);
            //Console.WriteLine(w2);

            //创建一个ProcessStartInfo对象 使用系统shell 指定命令和参数 设置标准输出
            //var psi = new ProcessStartInfo("dotnet", "--info") { RedirectStandardOutput = true };
            ////启动
            //var proc = Process.Start(psi);
            //if (proc == null)
            //{
            //    Console.WriteLine("Can not exec.");
            //}
            //else
            //{
            //    Console.WriteLine("-------------Start read standard output--------------");
            //    //开始读取
            //    using (var sr = proc.StandardOutput)
            //    {
            //        while (!sr.EndOfStream)
            //        {
            //            Console.WriteLine(sr.ReadLine());
            //        }

            //        if (!proc.HasExited)
            //        {
            //            proc.Kill();
            //        }
            //    }
            //    Console.WriteLine("---------------Read end------------------");
            //    Console.WriteLine($"Total execute time :{(proc.ExitTime - proc.StartTime).TotalMilliseconds} ms");
            //    Console.WriteLine($"Exited Code ： {proc.ExitCode}");
            //}

            //for (int j = 0; j < 10; j++)
            //    Console.WriteLine(DateTime.Now.Ticks);

            //Console.WriteLine(int.MaxValue);

            //var code = Guid.NewGuid().GetHashCode();
            //Console.WriteLine(code);

            //Console.WriteLine((int)(DateTime.Now.Ticks * code));

            #endregion

            ParameterExpression left = Expression.Parameter(typeof(int), "m");//int类型的，参数名称为m
            ConstantExpression right = Expression.Constant(10, typeof(int));//常量表达式树，10
            //进行左边是否大于右边的判断
            var binaryExpression = Expression.GreaterThan(left, right);

            Console.WriteLine(binaryExpression.ToString());

            var lambda = Expression.Lambda<Func<int, bool>>(binaryExpression, left).Compile();//构建成lambda表达式

            Console.WriteLine(lambda(5).ToString());

            Console.WriteLine("the end.");
            Console.Read();

        }

        static void Test(int i)
        {
            Console.WriteLine("thread-4:" + Thread.CurrentThread.ManagedThreadId);

            //await Task.Run(async () =>
            //{
            //    await Task.Delay(1000);
            //});

            //Console.WriteLine(i + ":" + Thread.CurrentThread.ManagedThreadId);

            //await Test2();

            Console.WriteLine("thread-5:" + Thread.CurrentThread.ManagedThreadId);
        }

        static void Test2()
        {
            Console.WriteLine("thread-2:" + Thread.CurrentThread.ManagedThreadId);

            var task = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);

                Console.WriteLine("thread-3:" + Thread.CurrentThread.ManagedThreadId);
            });

            task.Wait();
        }
    }
}