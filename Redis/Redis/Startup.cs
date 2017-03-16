using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Redis
{
    public class Startup
    {
        static StackExchange.Redis.ConnectionMultiplexer redis;
        static Startup()
        {
            Startup.redis = StackExchange.Redis.ConnectionMultiplexer.Connect(new StackExchange.Redis.ConfigurationOptions()
            {
                EndPoints = { { "localhost", 6379 } }, Password = "foobaar"
            });
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //http://dotnetthoughts.net/configuring-redis-for-aspnet-core-session-store/
            //https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "localhost, password=foobaar";
                options.InstanceName = "SampleInstance";
            });
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(30);
                options.CookieHttpOnly = true;
                options.CookieName = ".Redis.Session";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSession();

            app.Run(async (context) =>
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
                await db.StringIncrementAsync("hitcounter");
                await db.SetAddAsync("visitor", id.ToString());

                await context.Response.WriteAsync("<img src='/dotnetcore.png'><p>Hello World!<p>");
                
                var counter = await db.StringGetAsync("hitcounter");
                await context.Response.WriteAsync($"This page is visited {counter} times!<br>");
                var visitors = await db.SetLengthAsync("visitor");
                await context.Response.WriteAsync($"We served {visitors} unique visitors!");
            });
        }
    }
}
