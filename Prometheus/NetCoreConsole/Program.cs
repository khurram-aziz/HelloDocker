using Prometheus;
using System;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Threading;

namespace NetCoreConsole
{
    class Program
    {
        static bool keepRunning = false;
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
            AssemblyLoadContext.Default.Unloading += SigTermEventHandler; //register sigterm event handler. Don't forget to import System.Runtime.Loader!
            Console.CancelKeyPress += CancelHandler; //register sigint event handler

            var counter = Metrics.CreateCounter("custom_counter", "some help about this", new CounterConfiguration()
            {
                LabelNames = new[] { "name", "os" }
            });
            var gauge = Metrics.CreateGauge("custom_gauge", "some help about this", new GaugeConfiguration()
            {
                LabelNames = new[] { "name", "os" }
            });

            var thread = new Thread(new ThreadStart(() =>
            {
                do
                {
                    //gauge.Set(new Random().Next(0, 1000));
                    //counter.Inc();
                    //gauge.WithLabels(Environment.MachineName, Environment.OSVersion.ToString()).Set(new Random(Environment.TickCount).Next(0, 1000));
                    //OS Entropy
                    using (RNGCryptoServiceProvider rg = new RNGCryptoServiceProvider())
                    {
                        byte[] rno = new byte[5];
                        rg.GetBytes(rno);
                        int random = BitConverter.ToInt32(rno, 0);
                        gauge.WithLabels(Environment.MachineName, Environment.OSVersion.ToString()).Set(random);
                    }
                    counter.WithLabels(Environment.MachineName, Environment.OSVersion.ToString()).Inc();
                    Thread.Sleep(1000);
                } while (Program.keepRunning);
            }));

            var label = Metrics.CreateGauge("machine", "some help about this", new GaugeConfiguration()
            {
                LabelNames = new[] { "name", "os" }
            });
            label.WithLabels(Environment.MachineName, Environment.OSVersion.ToString()).Set(1);

            var metricServer = new MetricServer(port: 8000); //by default it publishes at /metrics
            var metricPusher = new MetricPusher("http://pushgateway:9091/metrics", job: Environment.MachineName);
            metricServer.Start();
            metricPusher.Start();

            thread.Start();

            Program.keepRunning = true;

            do
            {
                Thread.Sleep(1000);
                //Console.WriteLine("I am alive...");
            } while (Program.keepRunning);

            try
            {
                thread.Abort();
            }
            catch { }
            try
            {
                metricServer.Stop();
            }
            catch { }
        }
    }
}
