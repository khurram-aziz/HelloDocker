//https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed

using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace RedisMvc
{
    public static class StartTimeHeaderMiddlewareExtensions
    {
        public static IApplicationBuilder UseStartTimeHeaderMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<StartTimeHeaderMiddleware>();
        }
    }

    public class StartTimeHeaderMiddleware
    {
        readonly RequestDelegate next;
        readonly IDistributedCache cache;

        public StartTimeHeaderMiddleware(RequestDelegate next, IDistributedCache cache)
        {
            this.next = next;
            this.cache = cache;
        }

        public async Task Invoke(HttpContext context)  
        {
            string startTimeString = "Not found.";
            var value = await this.cache.GetAsync("lastServerStartTime");
            if (value != null)
                startTimeString = Encoding.UTF8.GetString(value);
            context.Response.Headers.Append("Last-Server-Start-Time", startTimeString);
            await this.next.Invoke(context);
        }
    }
}