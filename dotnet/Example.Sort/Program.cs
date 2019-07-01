using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Example.Sort
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var input = new[] { 3, 6, 9, 1, 4, 0, 7, 2, 5, 8 };

            //var arr = Fun.Sort(input);
            //var arr = Fun.SelectSort(input);
            var arr = Fun.InsertSort(input);

            foreach (var i in arr)
            {
                Console.Write(i + " ");
            }

            Console.ReadLine();
        }
    }
}
