using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Example.Redis
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!" + DateTime.MinValue);

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("192.168.186.73:6379");

            IDatabase db = redis.GetDatabase();

            //string value = "abcdefg";
            //db.StringSet("mykey", value);

            //Console.WriteLine(db.StringGet("mykey"));

            //db.StringAppend("mykey", "append");

            //Console.WriteLine(db.StringGet("mykey"));

            //db.StringSetRange("mykey", 5, "range");

            //Console.WriteLine(db.StringGet("mykey"));

            //var values = new List<HashEntry> { };

            //for (var i = 0; i <= 5; i++)
            //{
            //    HashEntry entry = new HashEntry("k_" + i, "vv_" + i);
            //    values.Add(entry);
            //}

            //db.HashSet("hash", values.ToArray());

            //var hResult = db.HashGet("hash", "k_5");
            //Console.WriteLine(hResult);

            //var list = db.HashGetAll("hash");
            //foreach (var item in list)
            //{
            //    Console.WriteLine(item.Value);
            //}

            //db.HashIncrement("hashset", "index");

            db.ListLeftPush("list", new RedisValue[] { 1, 2, 3, 4 });
            Console.WriteLine(db.ListRightPop("list"));

            //var j = db.StringIncrement("auto");
            //Console.WriteLine(j);

            //var tran = db.CreateTransaction();
            //tran.StringSetAsync("tran", "123456", TimeSpan.FromMinutes(1));
            //tran.StringSetAsync("tran2", "1234562", TimeSpan.FromMinutes(2));
            //tran.Execute();

            //db.SortedSetAdd("sort", "score1", 1.0);
            //db.SortedSetAdd("sort", "score3", 3.0);
            //db.SortedSetAdd("sort", "score2", 2.0);

            //Console.WriteLine(db.KeyRandom());

            //var list = db.SortedSetRangeByRank("sort", 0, 3, Order.Descending);
            //foreach (var item in list)
            //{
            //    Console.WriteLine(item);
            //}

            Console.WriteLine("the end.");
            Console.Read();
        }
    }
}
