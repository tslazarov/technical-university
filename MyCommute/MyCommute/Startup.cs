using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyCommute.Data;
using MyCommute.Data.Contracts;
using MyCommute.Data.Managers;
using MyCommute.Extensions.Hangfire;
using MyCommute.Extensions.Localization;
using MyCommute.Models;
using MyCommute.Services;
using System;

namespace MyCommute
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        { 
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddIdentity<User, IdentityRole>();
            services.AddHttpClient();

            services.AddDbContext<MyCommuteContext>(configurations =>
            {
                configurations.UseSqlServer(Configuration.GetConnectionString("MyCommuteConnectionString"));
            });

            RegisterDependencies(services);

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = "Temporary";
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddFacebook(options =>
            {
                options.AppId = Configuration.GetValue<string>("Authentication:Facebook:Key").ToString();
                options.AppSecret = Configuration.GetValue<string>("Authentication:Facebook:Secret").ToString();
            })
            .AddGoogle(options =>
            {
                options.ClientId = Configuration.GetValue<string>("Authentication:Google:Key").ToString();
                options.ClientSecret = Configuration.GetValue<string>("Authentication:Google:Secret").ToString();
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/auth/signin";
            })
            .AddCookie("Temporary");

            services.AddHangfire(config => config.UseSqlServerStorage(Configuration.GetConnectionString("MyCommuteHangifreConnectionString")));


            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint));
                options.LowercaseUrls = true;
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(options => options.ResourcesPath = "Resources")
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            services.AddScoped<DbContext, MyCommuteContext>();
            services.AddScoped<IData, MyCommuteData>();
            services.AddScoped<IEfRepository<User>, EfRepository<User>>();
            services.AddScoped<IEfRepository<Car>, EfRepository<Car>>();
            services.AddScoped<IEfRepository<Fuel>, EfRepository<Fuel>>();
            services.AddScoped<IEfRepository<Ride>, EfRepository<Ride>>();
            services.AddScoped<IUsersManager, UsersManager>();
            services.AddScoped<ICarsManager, CarsManager>();
            services.AddScoped<IFuelsManager, FuelsManager>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<FuelPriceFetcher>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new[] { new CustomDashboardAuthorizationFilter() }
            });

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "LocalizedDefault",
                    template: "{lang:lang=bg}/{controller=Home}/{action=Index}/{id?}"
                );
                routes.MapRoute(
                    name: "Default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );
            });

            var fetcher = serviceProvider.GetService<FuelPriceFetcher>();
            RecurringJob.AddOrUpdate("fuel-price-fetcher", () => fetcher.FetchData(), "* 10 * * *");

            lifetime.ApplicationStarted.Register(OnApplicationStarted);
            lifetime.ApplicationStopped.Register(OnApplicationStopped);
        }

        public void OnApplicationStarted()
        {
        }

        public void OnApplicationStopped()
        {
        }
    }
}
