using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;
using System;
using System.Text;

namespace KafkaNetCoreConsole
{
    class Program
    {
        static void producer()
        {
            var options = new KafkaOptions(new Uri("http://kafka:9092"));
            var router = new BrokerRouter(options);
            var client = new Producer(router);
            client.SendMessageAsync("testtopic", new[] {
                new Message("Hi Hello! Welcome to Kafka!") }).Wait();
        }

        static void consumer()
        {
            var options = new KafkaOptions(new Uri("http://kafka:9092"));
            var router = new BrokerRouter(options);
            var consumer = new Consumer(new ConsumerOptions("testtopic",
                new BrokerRouter(options)));

            //Consume returns a blocking IEnumerable (ie: never ending stream)
            foreach (var message in consumer.Consume())
            {
                Console.WriteLine("Response: P{0},O{1} : {2}",
                    message.Meta.PartitionId, message.Meta.Offset,
                    Encoding.UTF8.GetString(message.Value));
            }
        }

        static void Main(string[] args)
        {
            Program.producer();
            Program.consumer();
            //Console.ReadLine();
        }
    }
}
