using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
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

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(
                    _configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Tutorial.Web")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<IdentityDbContext>();
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });
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
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/node_modules",
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules"))
            });
            //app.UseFileServer(); //结合的上面的两个
            /*app.UseMvcWithDefaultRoute();*/

            app.UseAuthentication();
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
