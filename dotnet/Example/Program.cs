using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Console.WriteLine("thread-1:" + Thread.CurrentThread.ManagedThreadId);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < 20; i++)
            {
                Test2();
                i++;
            }

            //await TestAsync(0);

            stopwatch.Stop();

            Console.WriteLine("time:" + stopwatch.ElapsedMilliseconds + "ms");

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

            Console.Read();

        }

        enum Day : byte { Sat = 1, Sun, Mon, Tue, Wed, Thu, Fri };

        class YieldTest
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

        static async Task TestAsync(int i)
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
