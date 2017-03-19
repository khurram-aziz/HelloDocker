using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace RedisMvc
{
    public static class RedisVisitorMiddlewareExtensions
    {
        public static IApplicationBuilder UseRedisVisitorMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RedisVisitorMiddleware>();
        }
    }

    public class RedisVisitorMiddleware
    {
        readonly RequestDelegate next;

        public RedisVisitorMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)  
        {
            Guid id;
            if (context.Request.Cookies["visitor"] == null)
            {
                id = Guid.NewGuid();
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddYears(10);
                context.Response.Cookies.Append("visitor", id.ToString());
            }
            else
                id = Guid.Parse(context.Request.Cookies["visitor"]);

            //Ideally this should use IDsitributedCache so it can be used with other
            //distributed cache providers like SQL; but given we are using Redis Set
            //operations; therefore this middleware directly depends on Redis
            var db = Startup.RedisConnection.GetDatabase();
            await db.SetAddAsync("visitor", id.ToString());
            
            await this.next.Invoke(context);
        }
    }
}
