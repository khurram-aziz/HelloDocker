using Prometheus;
using System;
using System.Threading;

namespace NetCoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var counter = Metrics.CreateCounter("custom_counter", "some help about this");
            var gauge = Metrics.CreateGauge("custom_gauge", "some help about this");

            var thread = new Thread(new ThreadStart(() =>
            {
                gauge.Set(new Random().Next(0, 1000));
                counter.Inc();
                Thread.Sleep(1000);
            }));

            var label = Metrics.CreateGauge("machine", "some help about this", new GaugeConfiguration()
            {
                LabelNames = new[] { "name", "os" }
            });
            label.WithLabels(Environment.MachineName, Environment.OSVersion.ToString()).Set(1);

            var metricServer = new MetricServer(port: 8000); //by default it publishes at /metrics
            metricServer.Start();

            thread.Start();

            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();

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
