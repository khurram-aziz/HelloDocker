using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Text;
using System.Threading;

namespace KafkaNetCoreConsole
{
    //Other options https://github.com/Microsoft/CSharpClient-for-Kafka
    class Program
    {
		static bool keepRunning = true;

		static void SigTermEventHandler(AssemblyLoadContext obj)
        {
            Program.keepRunning = false;
            Console.WriteLine("Unloading...");
        }

        static void CancelHandler(object sender, ConsoleCancelEventArgs e)
        {
            Program.keepRunning = false;
            Console.WriteLine("Exiting...");
        }

        static void Main(string[] args)
        {
            AssemblyLoadContext.Default.Unloading += SigTermEventHandler;
            Console.CancelKeyPress += CancelHandler;

			var machine = string.Format(@"{0}\{1}", Environment.MachineName, Environment.OSVersion);

            Thread.Sleep(5000);
			if (args[0] == "producer")
			{
                Thread.Sleep(5000);
				long counter = 0;
				var key = args[1];
                var config = new Dictionary<string, object>
                {
                    { "bootstrap.servers", "kafka:9092" }
                };

                do
                {
					counter++;
                    using (var producer = new Producer<Null, string>(
                        config, null /*keySerializer*/,
                        new StringSerializer(Encoding.UTF8)))
                    {
                        string text = "Hi Hello! Welcome to Kafka from " + machine + "!";
                        foreach (var k in key.Split(",".ToCharArray()))
                        {
                            producer.ProduceAsync("testtopic", null /*key*/, text, int.Parse(k));
                            producer.Flush(100);
                            Console.WriteLine("Produced into " + k);
                        }
                    }

					Thread.Sleep(5000);
					if (args.Length >= 3 && counter >= int.Parse(args[2]))
						break;
				} while(Program.keepRunning);
			}
			else if (args[0] == "consumer")
			{
				var key = args[1];
                var config = new Dictionary<string, object>
                {
                    { "bootstrap.servers", "kafka:9092" },
                    { "group.id", "sample-consumer" },
                    { "enable.auto.commit", "false" },
                    { "statistics.interval.ms", 60000 }, //we have subscribed to stats below
                    //{ "auto.commit.interval.ms", 5000 },
                    { "auto.offset.reset", "smallest" } // earliest
                };
                using (var consumer = new Consumer<Null, string>(
                    config, null /*keyDeserializer*/,
                    new StringDeserializer(Encoding.UTF8)))
                {
                    Console.WriteLine("Consumer({0}) setting up!", key);

                    var ps = new List<TopicPartition>();
                    foreach (var k in key.Split(",".ToCharArray()))
                        ps.Add(new TopicPartition("testtopic", int.Parse(k)));

                    //if we dont subscribe to this event; assigned partitions are passed to consumer automatically
                    consumer.OnPartitionsAssigned += (_, partitions) =>
                    {
                        Console.WriteLine("Assigning " + key + " to consumer!");
                        consumer.Assign(ps.ToArray());
                    };
                    consumer.OnStatistics += (_, json) => Console.WriteLine($"Statistics: {json}");
                    //consumer.OnPartitionsRevoked += (_, partitions) => consumer.Unassign();

                    consumer.OnMessage += (_, msg) =>
                    {
                        Console.WriteLine("Consumer({0}): P{1},O{2} : {3}",
                            key, msg.Partition, msg.Offset.Value, msg.Value);
                        consumer.CommitAsync(msg);
                    };
                    consumer.OnError += (_, error)
                        => Console.WriteLine($"Error: {error}");
                    consumer.OnConsumeError += (_, msg)
                        => Console.WriteLine($"Consume error ({msg.TopicPartitionOffset}): {msg.Error}");
                    consumer.Subscribe(new string[] { "testtopic" });

                    while (Program.keepRunning)
                    {
                        consumer.Poll(100);
                    }
                }
            }
            Console.WriteLine("Finished!");
        }
    }
}
