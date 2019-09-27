using System;
using System.Collections.Generic;
using System.Threading;

namespace Example.DistributedId
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //IdWorker idWorker = new IdWorker(0, 0);

            //int i = 00;
            //while (i < 20)
            //{
            //    long id = idWorker.nextId();
            //    Console.WriteLine(id);
            //    i++;
            //}

            int i = 00;
            while (i < 20)
            {
                IdCreator c = new IdCreator(0, 16);
                var id = c.Create();
                Console.WriteLine(id);


                i++;
            }

            Console.WriteLine("The End.");
            Console.Read();
        }
    }
}
