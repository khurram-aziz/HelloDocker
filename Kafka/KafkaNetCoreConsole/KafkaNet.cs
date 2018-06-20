using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;
using System;
using System.Text;
using System.Threading;

namespace KafkaNetCoreConsole
{
    class MyPartition : IPartitionSelector
    {
        Partition p = null;

        public MyPartition(int partitionId)
        {
            this.p = new Partition();
            p.PartitionId = partitionId;
        }
        public Partition Select(Topic topic, byte[] key)
        {
            return this.p;
        }
    }

    class KafkaNetProgram
    {
        static public void KafkaNetMain(string machineID, string mode, string topic, string key, int? howMany)
        {
            if (mode == "producer")
            {
                long counter = 0;
                do
                {
                    counter++;
                    foreach (var k in key.Split(",".ToCharArray()))
                    {
                        var options = new KafkaOptions(new Uri("http://kafka:9092"));
                        options.PartitionSelector = new MyPartition(int.Parse(k));
                        var router = new BrokerRouter(options);
                        using (var client = new Producer(router))
                        {
                            client.SendMessageAsync(topic, new[] {
                                new Message($"Message {counter} from {machineID}")}).Wait();
                            Console.WriteLine("Produced into " + k);
                        }
                    }
                    Thread.Sleep(1000);
                    if (howMany.HasValue && counter >= howMany.Value)
                        break;
                } while (Program.KeepRunning);
            }
            else if (mode == "consumer")
            {
                var options = new KafkaOptions(new Uri("http://kafka:9092"));
                var router = new BrokerRouter(options);
                var consumerOptions = new ConsumerOptions(topic, new BrokerRouter(options));
                foreach (var k in key.Split(",".ToCharArray()))
                    consumerOptions.PartitionWhitelist.Add(int.Parse(k));
                using (var consumer = new Consumer(consumerOptions))
                {
                    Console.WriteLine("Consumer({0}) setting up!", key);
                    //Consume is blocking; we are getting never ending stream
                    foreach (var message in consumer.Consume(/*ct*/))
                    {
                        Console.WriteLine("Consumer({0}): P{1},O{2} : {3}",
                            key, message.Meta.PartitionId, message.Meta.Offset,
                            Encoding.UTF8.GetString(message.Value));
                    }
                }
            }
            Console.WriteLine("Finished!");
        }
    }
}
