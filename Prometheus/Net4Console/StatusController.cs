using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Net4Console
{
    public class StatusController : ApiController
    {
        public static int Counter = 0;
        public static float Gauge = 0;

        public HttpResponseMessage Get()
        {
            var html = new StringBuilder();

            html.Append(@"<html><head><title>Status</title><meta http-equiv=""refresh"" content=""30""></head><body>");
            html.Append(string.Format("<p>Machine Name: {0}<br>Operating System: {1}</p>", Environment.MachineName, Environment.OSVersion));

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(html.ToString(), Encoding.UTF8, "text/html");
            return resp;
        }

        [HttpGet]
        public HttpResponseMessage Metrics()
        {
            StatusController.Counter++;
            StatusController.Gauge = (float)(new Random().Next(0, 100000)) / (float)(new Random().Next(1, 100));

            var sb = new StringBuilder();

            sb.Append(string.Format(@"machine{{name=""{1}"",os=""{2}""}} 1{0}", Environment.NewLine, Environment.MachineName, Environment.OSVersion));
            sb.Append(string.Format(@"custom_counter {1}{0}custom_gauge {2}", Environment.NewLine, StatusController.Counter, StatusController.Gauge));

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(sb.ToString(), Encoding.UTF8, "text/plain"); //"text/plain; version=0.0.4" gives invalid
            return resp;
        }
    }
}
