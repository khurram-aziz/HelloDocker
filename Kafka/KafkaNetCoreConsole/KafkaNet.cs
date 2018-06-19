using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;
using System;
using System.Runtime.Loader;
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
        static bool keepRunning = true;

        static void SigTermEventHandler(AssemblyLoadContext obj)
        {
            KafkaNetProgram.keepRunning = false;
            Console.WriteLine("Unloading...");
        }

        static void CancelHandler(object sender, ConsoleCancelEventArgs e)
        {
            KafkaNetProgram.keepRunning = false;
            Console.WriteLine("Exiting...");
        }

        static void KafkaNetMain(string[] args)
        {
            AssemblyLoadContext.Default.Unloading += SigTermEventHandler;
            Console.CancelKeyPress += CancelHandler;

            var machine = string.Format(@"{0}\{1}", Environment.MachineName, Environment.OSVersion);
            Thread.Sleep(5000);

            if (args[0] == "producer")
            {
                long counter = 0;
                var key = args[1];

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
                            client.SendMessageAsync("testtopic", new[] {
                                new Message("Hi Hello! Welcome to Kafka from " + machine + "!")}).Wait();
                            Console.WriteLine("Produced into " + k);
                        }
                    }
                    Thread.Sleep(1000);
                    if (args.Length >= 3 && counter >= int.Parse(args[2]))
                        break;
                } while (KafkaNetProgram.keepRunning);
            }
            else if (args[0] == "consumer")
            {
                var key = args[1];
                var options = new KafkaOptions(new Uri("http://kafka:9092"));
                var router = new BrokerRouter(options);
                var consumerOptions = new ConsumerOptions("testtopic", new BrokerRouter(options));
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
