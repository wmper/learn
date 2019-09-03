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
            Console.WriteLine("Hello World!");

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("192.168.186.73:6379");
            IDatabase db = redis.GetDatabase();

            string value = "abcdefg";
            db.StringSet("mykey", value);

            string result = db.StringGet("mykey");
            Console.WriteLine(result);

            var values = new List<HashEntry> { };

            for (var i = 0; i <= 10; i++)
            {
                HashEntry entry = new HashEntry("k_" + i, "v_" + i);
                values.Add(entry);
            }

            db.HashSet("hash", values.ToArray());

            var hResult = db.HashGet("hash", "k_5");
            Console.WriteLine(hResult);


            db.ListLeftPush("list", new RedisValue[] { 1, 2, 3, 4 });
            Console.WriteLine(db.ListRightPop("list"));

            var j = db.StringIncrement("auto");
            Console.WriteLine(j);

            Console.Read();
        }
    }
}
