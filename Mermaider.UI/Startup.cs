namespace Mermaider.UI
{
    using System;
    using System.IO;
    using Core;
    using Core.Abstractions;
    using Core.IO;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        private IHostingEnvironment _hostingEnvironment;

        public Startup(IHostingEnvironment env)
        {
            _hostingEnvironment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var mgrConfig = new ManagerConfig();
            Configuration.GetSection("ManagerConfig").Bind(mgrConfig);
            
            var unsavedGraphFilesPath = Path.Combine(_hostingEnvironment.WebRootPath, mgrConfig.UnsavedGraphFilesPath);
            var savedGraphFilesPath = Path.Combine(_hostingEnvironment.WebRootPath, mgrConfig.SavedGraphFilesPath);
            new FileUtils().CreateDir(unsavedGraphFilesPath,savedGraphFilesPath);

            mgrConfig.UnsavedGraphFilesPath = unsavedGraphFilesPath;
            mgrConfig.SavedGraphFilesPath = savedGraphFilesPath;
            

            var mgr = new Manager();
            mgr.Configure(mgrConfig);
            services.AddSingleton<IManager>(mgr);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=LiveEditor}/{action=Index}/{id?}");
            });
            app.Run(async context => { await context.Response.WriteAsync("Starting up Mermaider... (or a script/css/image/other resource file path is wrong)"); });
        }
    }
}