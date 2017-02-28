using org.apache.zookeeper;
using System;
using System.Text;

namespace Zoo
{
    class HelloZoo
    {
        public static void CreateAndRead()
        {
            var watcher = NullWatcher.Instance;
            //start the docker container mapping the 2181 port to host
            var zk = new ZooKeeper("127.0.0.1:2181", 5000, watcher);
            string rootNode = zk.createAsync("/dotnetcoreapp",
                Encoding.ASCII.GetBytes("My Zookeeper Dotnet Core App"), ZooDefs.Ids.OPEN_ACL_UNSAFE,
                CreateMode.PERSISTENT).Result;
            string ephNode = zk.createAsync("/dotnetcoreapp/ephNode",
                Encoding.ASCII.GetBytes("Ephemeral Value"), ZooDefs.Ids.OPEN_ACL_UNSAFE,
                CreateMode.EPHEMERAL).Result;
            string ephSeqNode = zk.createAsync("/dotnetcoreapp/ephSeqNode",
                Encoding.ASCII.GetBytes("Ephemeral Sequential Value"), ZooDefs.Ids.OPEN_ACL_UNSAFE,
                CreateMode.EPHEMERAL_SEQUENTIAL).Result;
            string perNode = zk.createAsync("/dotnetcoreapp/perNode",
                Encoding.ASCII.GetBytes("Persistent Value"), ZooDefs.Ids.OPEN_ACL_UNSAFE,
                CreateMode.PERSISTENT).Result;
            string perSeqNode = zk.createAsync("/dotnetcoreapp/perSeqNode",
                Encoding.ASCII.GetBytes("Persistent Sequential Value"), ZooDefs.Ids.OPEN_ACL_UNSAFE,
                CreateMode.PERSISTENT_SEQUENTIAL).Result;

            Console.WriteLine("Ephemeral: " + ephNode);
            Console.WriteLine("Ephemeral Sequential: " + ephSeqNode);
            Console.WriteLine("Persistent: " + perNode);
            Console.WriteLine("Persistent Sequential: " + perSeqNode);

            var dr1 = zk.getDataAsync(ephNode).Result;
            var dr2 = zk.getDataAsync(ephSeqNode).Result;
            var dr3 = zk.getDataAsync(perNode).Result;
            var dr4 = zk.getDataAsync(perSeqNode).Result;
            string data1 = Encoding.ASCII.GetString(dr1.Data);
            string data2 = Encoding.ASCII.GetString(dr2.Data);
            string data3 = Encoding.ASCII.GetString(dr3.Data);
            string data4 = Encoding.ASCII.GetString(dr4.Data);

            Console.WriteLine("Ephemeral: " + data1);
            Console.WriteLine("Ephemeral Sequential: " + data2);
            Console.WriteLine("Persistent: " + data3);
            Console.WriteLine("Persistent Sequential: " + data4);
        }
    }
}