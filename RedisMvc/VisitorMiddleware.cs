using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Redis
{
    public class VisitorMiddleware
    {
        readonly RequestDelegate next;
        public VisitorMiddleware(RequestDelegate next)
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

            var db = Startup.redis.GetDatabase();
            await db.SetAddAsync("visitor", id.ToString());
            
            await this.next.Invoke(context);
        }
    }
}
