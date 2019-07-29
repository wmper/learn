using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("thread-1:" + Thread.CurrentThread.ManagedThreadId);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //for (int i = 0; i < 10; i++)
            //{
            //    await Test(i);
            //}

            await TestAsync(0);

            stopwatch.Stop();

            Console.WriteLine("time:" + stopwatch.ElapsedMilliseconds + "ms");


            Console.Read();
        }

        static async Task TestAsync(int i)
        {
            Console.WriteLine("thread-4:" + Thread.CurrentThread.ManagedThreadId);

            //await Task.Run(async () =>
            //{
            //    await Task.Delay(1000);
            //});

            //Console.WriteLine(i + ":" + Thread.CurrentThread.ManagedThreadId);

            await Test2();

            Console.WriteLine("thread-5:" + Thread.CurrentThread.ManagedThreadId);
        }

        static Task Test2()
        {
            Console.WriteLine("thread-2:" + Thread.CurrentThread.ManagedThreadId);

            return Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);

                Console.WriteLine("thread-3:" + Thread.CurrentThread.ManagedThreadId);
            });
        }
    }
}
