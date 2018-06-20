using System;
using System.Runtime.Loader;
using System.Threading;

namespace KafkaNetCoreConsole
{
    //Other options https://github.com/Microsoft/CSharpClient-for-Kafka
    class Program
    {
        static public bool KeepRunning = true;

        static void SigTermEventHandler(AssemblyLoadContext obj)
        {
            Program.KeepRunning = false;
            Console.WriteLine("Unloading...");
        }

        static void CancelHandler(object sender, ConsoleCancelEventArgs e)
        {
            Program.KeepRunning = false;
            Console.WriteLine("Exiting...");
        }

        static void Main(string[] args)
        {
            AssemblyLoadContext.Default.Unloading += SigTermEventHandler;
            Console.CancelKeyPress += CancelHandler;

            var machine = string.Format(@"{0}\{1}", Environment.MachineName, Environment.OSVersion);
            var mode = args[0];     //producer|consumer
            var topic = args[1];
            var key = args[2];      //partitions eg 0 1,2
            int? howMany = null;
            if (args.Length >= 4) howMany = int.Parse(args[3]);

            Thread.Sleep(5000); //let zookeeper + kafka come online
            Thread.Sleep(5000); //let setup initialize the topic
            ConfluentKafkaProgram.ConfluentKafkaMain(machine, mode, topic, key, howMany);
        }
    }
}
