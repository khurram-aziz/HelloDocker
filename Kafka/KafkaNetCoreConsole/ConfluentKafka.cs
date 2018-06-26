using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace KafkaNetCoreConsole
{
    class ConfluentKafkaProgram
    {
        static public void ConfluentKafkaMain(string machineID, string mode, string topic, string key, int? howMany)
        {
            if (mode == "producer")
            {
                long counter = 0;
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
                        string text = $"Message {counter} from {machineID}";
                        foreach (var k in key.Split(",".ToCharArray()))
                        {
                            producer.ProduceAsync(topic, null /*key*/, text, int.Parse(k));
                            producer.Flush(100);
                            Console.WriteLine($"Produced {counter} into {k}");
                        }
                    }

                    Thread.Sleep(5000);
                    if (howMany.HasValue && counter >= howMany.Value)
                        break;
                } while (Program.KeepRunning);
            }
            else if (mode == "consumer")
            {
                var config = new Dictionary<string, object>
                {
                    { "bootstrap.servers", "kafka:9092" },
                    { "group.id", "sample-consumer" },
                    { "enable.auto.commit", "false" },
                    //{ "statistics.interval.ms", 60000 }, //we have subscribed to stats below
                    //{ "auto.commit.interval.ms", 5000 },
                    { "auto.offset.reset", "smallest" } // smallest; earliest
                };
                using (var consumer = new Consumer<Null, string>(
                    config, null /*keyDeserializer*/,
                    new StringDeserializer(Encoding.UTF8)))
                {
                    Console.WriteLine("Consumer({0}) setting up!", key);

                    long counter = 0;

                    //if we dont subscribe to this event; assigned partitions are passed to consumer automatically
                    consumer.OnPartitionsAssigned += (_, partitions) =>
                    {
                        if (!string.IsNullOrEmpty(key))
                        {
                            Console.WriteLine($"Assigning {key} to consumer!");
                            var ps = new List<TopicPartition>();
                            foreach (var k in key.Split(",".ToCharArray()))
                                ps.Add(new TopicPartition(topic, int.Parse(k)));
                            consumer.Assign(ps.ToArray());
                        }
                        else
                        {
                            string pstr = "";
                            foreach(var p in partitions)
                            {
                                pstr += p.Partition.ToString();
                                if (!string.IsNullOrEmpty(pstr))
                                    pstr += ",";
                            }
                            Console.WriteLine($"Server assigned {key} to consumer!");
                        }
                    };
                    consumer.OnStatistics += (_, json) => Console.WriteLine($"Statistics: {json}");
                    //consumer.OnPartitionsRevoked += (_, partitions) => consumer.Unassign();

                    consumer.OnMessage += (_, msg) =>
                    {
                        counter++;
                        Console.WriteLine("Consumer({0}): P{1},O{2} : {3}",
                            key, msg.Partition, msg.Offset.Value, msg.Value);
                        Thread.Sleep(3000); //simulating processing
                        // we can simulate failure here; if (counter >= 2) Environment.Exit(-1);
                        consumer.CommitAsync(msg);
                    };
                    consumer.OnError += (_, error)  => Console.WriteLine($"Error: {error}");
                    consumer.OnConsumeError += (_, msg) => Console.WriteLine($"Consume error ({msg.TopicPartitionOffset}): {msg.Error}");
                    consumer.Subscribe(new string[] { topic });

                    while (Program.KeepRunning)
                    {
                        consumer.Poll(100);
                    }
                }
            }
            Console.WriteLine("Finished!");
        }
    }
}