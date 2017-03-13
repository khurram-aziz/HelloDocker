using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RedisMvc
{
    public class Startup
    {
        internal static StackExchange.Redis.ConnectionMultiplexer Redis;
        static Startup()
        {
            Startup.Redis = StackExchange.Redis.ConnectionMultiplexer.Connect(new StackExchange.Redis.ConfigurationOptions()
            {
                //We need redis to be resolving
                EndPoints = { { "localhost", 6379 } },
                Password = "foobaar"
            });
        }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IDistributedCache cache)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();

            //https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed
            var serverStartTimeString = DateTime.Now.ToString();
            byte[] val = Encoding.UTF8.GetBytes(serverStartTimeString);
            var cacheEntryOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(30)); // Set a short timeout for easy testing.
            cache.Set("lastServerStartTime", val, cacheEntryOptions);
            app.UseStartTimeHeaderMiddleware();

            app.UseRedisVisitorMiddleware();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
