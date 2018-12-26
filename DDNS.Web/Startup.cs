using DDNS.DataModel.LoginLog;
using DDNS.DataModel.Tunnel;
using DDNS.DataModel.Users;
using DDNS.DataModel.Verify;
using DDNS.Entity;
using DDNS.Entity.AppSettings;
using DDNS.Provider.LoginLog;
using DDNS.Provider.Tunnel;
using DDNS.Provider.Users;
using DDNS.Provider.Verify;
using DDNS.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace DDNS.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(env.ContentRootPath)
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie();
            services.AddSession();
            services.AddOptions();

            services.Configure<EmailConfig>(Configuration.GetSection("EmailConfig"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<EmailUtil>();
            services.AddScoped<UsersProvider>();
            services.AddScoped<UsersDataModel>();
            services.AddScoped<VerifyProvider>();
            services.AddScoped<VerifyDataModel>();
            services.AddScoped<LoginLogProvider>();
            services.AddScoped<LoginLogDataModel>();
            services.AddScoped<TunnelProvider>();
            services.AddScoped<TunnelDataModel>();

            services.AddDbContext<DDNSDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("DDNSConnection"));
            });

            services.AddRouting(routes =>
            {
                routes.LowercaseUrls = true;
                routes.AppendTrailingSlash = false;
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("DDNS", null);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "DDNS.xml"));
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en-US"),
                        new CultureInfo("zh-CN")
                    };

                options.DefaultRequestCulture = new RequestCulture("zh-CN");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/DDNS/swagger.json", "DDNS API");
            });

            var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("zh-CN")
                };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("zh-CN"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            app.UseSession();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}