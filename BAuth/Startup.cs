using Formix.Authentication.Basic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;

namespace BAuth
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseBasicAuthentication(creds =>
            {
                Console.WriteLine("Authenticating User={0}", creds.UserName);
                if (creds.UserName == "khurram" || creds.Password == "khurram")
                {
                    Console.WriteLine(" Successful");
                    return new ClaimsPrincipal(new[] { new ClaimsIdentity(new[]
                    { new Claim(ClaimTypes.Name, creds.UserName)  }, "custom-auth-type") });
                }
                return null;
            }, "custom-realm", 3000);

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
