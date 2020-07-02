using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tutorial.Web.Data;
using Tutorial.Web.Model;
using Tutorial.Web.Services;

namespace Tutorial.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            /*var connectionString = _configuration["ConnectString:DefaultConnection"];*/

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddSingleton<IWelcomeService, WelcomeService>();
            services.AddSingleton<IRepository<Student>, InMemoryRepository>();
            services.AddScoped<IRepository<Student>, EFCoreRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env,
            IWelcomeService welcomeService,
            ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }
            //app.UseDefaultFiles();  不是伺服文件 会改变请求的路径
            app.UseStaticFiles();
            //app.UseFileServer(); //结合的上面的两个
            /*app.UseMvcWithDefaultRoute();*/
            app.UseMvc(builder =>
           {
               // /Home/Index/3 -> HomeController Index(3)

               /*conventional*/
               builder.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");

           });

            /*env.IsEnvironment("IntegrationTest");*/
            //app.Use(next => 
            //{
            //    logger.LogInformation("------app.Use()");
            //    return async HttpContext =>
            //    {
            //        logger.LogInformation("------async httpContext");
            //        if (HttpContext.Request.Path.StartsWithSegments("/first"))
            //        {
            //            logger.LogInformation("------First!!");
            //            await HttpContext.Response.WriteAsync("First!!");
            //        }
            //        else
            //        {
            //            logger.LogInformation("------next(httpContext)");
            //            await next(HttpContext);
            //        }
            //    };
            //});
            //app.UseWelcomePage(new WelcomePageOptions 
            //{
            //    Path="/welcome"
            //});


            app.Run(async (context) =>
            {
                /*throw new Exception("Error");*/
                var welcome = welcomeService.GetMessage();
                await context.Response.WriteAsync(welcome);
            });
        }
    }
}
