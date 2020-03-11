using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
            Console.WriteLine(_config.ToString());
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            app.Use(async (context, next) => 
            {
                logger.LogInformation("MW1: Incoming Request");
                //await context.Response.WriteAsync("Hello Word \n");
                await next();
                logger.LogInformation("MW1: OutGoing Request");

            });
            app.Use(async (context, next) =>
            {
                logger.LogInformation("MW2: Incoming Request");
                //await context.Response.WriteAsync("Hello Word \n");
                await next();
                logger.LogInformation("MW2: OutGoing Request");

            });
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello From Second Middleware");
                logger.LogInformation("MW3: Request Handled and response produced");
            });
            //app.UseHttpsRedirection();
            //app.UseStaticFiles();
            //app.UseRouting();
            //app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});
        }
    }
}
