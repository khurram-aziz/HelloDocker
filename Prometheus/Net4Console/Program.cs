using System;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace Net4Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string url = @"http://localhost:8000";
                var config = new HttpSelfHostConfiguration(url);
                config.Routes.MapHttpRoute("API Default",
                    "api/{controller}/{action}/{id}",
                    new { id = RouteParameter.Optional });
                config.Routes.MapHttpRoute("Status", "status", new { controller = "Status" });
                config.Routes.MapHttpRoute("Metrics", "metrics", new { controller = "Status", action = "Metrics" });
                HttpSelfHostServer server = new HttpSelfHostServer(config);
                server.OpenAsync().Wait();

                Console.WriteLine("Web server started at {0}", url);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to setup web server");
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }
    }
}
