using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
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
using MyCommute.Extensions;
using MyCommute.Models;
using MyCommute.Services;

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

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(options => options.ResourcesPath = "Resources")
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    var supportedCultures = new List<CultureInfo>
            //    {
            //        new CultureInfo("en"),
            //        new CultureInfo("bg"),
            //    };
            //    options.DefaultRequestCulture = new RequestCulture("bg");
            //    options.SupportedCultures = supportedCultures;
            //    options.SupportedUICultures = supportedCultures;
            //    var provider = new RouteDataRequestCultureProvider();
            //    provider.RouteDataStringKey = "lang";
            //    provider.UIRouteDataStringKey = "lang";
            //    provider.Options = options;
            //    options.RequestCultureProviders = new[] { provider };
            //});

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint));
            });
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
            services.AddScoped<IUsersService, UsersService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );
                routes.MapRoute(
                    name: "LocalizedDefault",
                    template: "{lang:lang=bg}/{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
