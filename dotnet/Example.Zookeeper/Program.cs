using org.apache.zookeeper;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Example.Zookeeper
{
    class Program
    {
        private const string hostPort = "127.0.0.1,localhost";
        public const int CONNECTION_TIMEOUT = 4000;

        static async Task Main(string[] args)
        {
            var watcher = NullWatcher.Instance;

            ZooKeeper zk = new ZooKeeper(hostPort, CONNECTION_TIMEOUT, watcher);
            var root = await zk.createAsync("/root", Encoding.UTF8.GetBytes("123456"), null, CreateMode.EPHEMERAL);

            Console.WriteLine(zk.getDataAsync("/root"));

            await Task.CompletedTask;

            Console.ReadKey();
        }
    }

    public class NullWatcher : Watcher
    {
        public static readonly NullWatcher Instance = new NullWatcher();
        private NullWatcher() { }
        public override Task process(WatchedEvent @event)
        {
            return Task.CompletedTask;
            // nada
        }
    }
}
