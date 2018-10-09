using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hover.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Hover
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            ConfigureMe = config;
        }

        public IConfiguration ConfigureMe { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            services.AddOptions();
            services.Configure<Settings>(ConfigureMe.GetSection("Settings"));

            // MySqlConnector
            services.AddTransient<HoverDb>(_ => new HoverDb(ConfigureMe.GetConnectionString("hoverConn")));

            services.AddTransient<Settings>();

            services.AddDataProtection()
                // point at a specific folder and use DPAPI to encrypt keys
                .SetApplicationName(ConfigureMe.GetValue<string>("Config:AppName"))
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo(ConfigureMe.GetValue<string>("Config:PersistKeysToFileLocation")))
                .ProtectKeysWithDpapi(protectToLocalMachine: true);

            //services.Configure<MvcOptions>(options =>
            //{
            //    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Cors.Internal.CorsAuthorizationFilterFactory("StaticSitePolicy"));
            //});
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddMvcCore(options =>
                {
                    options.RequireHttpsPermanent = true;
                })
                // .AddApiExplorer()
                // .AddAuthorization()
                .AddFormatterMappings()
                .AddCors(options =>
                {
                    options.AddPolicy("AllowStaticSite", GenerateCorsPolicy());
                })
                .AddJsonFormatters();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCors("AllowStaticSite");

            app.UseHttpsRedirection();
            //app.UseDefaultFiles();
            //app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMvc();
        }
        public CorsPolicy GenerateCorsPolicy()
        {
            var corsBuilder = new CorsPolicyBuilder();
                corsBuilder.WithOrigins("https://" + ConfigureMe.GetValue<string>("Settings:StaticSiteRoot"));
                corsBuilder.AllowAnyMethod();
                corsBuilder.AllowAnyHeader();
                corsBuilder.AllowCredentials();
            return corsBuilder.Build();
        }
    }
}
