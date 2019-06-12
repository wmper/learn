using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooKeeperNet;

namespace Example.Zookeeper.FK
{
    class Program
    {
        static void Main(string[] args)
        {
            IZooKeeper zk = new ZooKeeper("127.0.0.1:2181", TimeSpan.FromHours(1), null);
            var root = zk.Create("/root", Encoding.UTF8.GetBytes("123456"), null, CreateMode.Ephemeral);

            Console.WriteLine(root);

            var t = zk.GetData("/root", null, null);
            Console.WriteLine(t);

            Console.ReadKey();

        }
    }
}
