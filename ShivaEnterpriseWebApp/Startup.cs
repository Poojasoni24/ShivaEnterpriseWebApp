using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using ShivaEnterpriseWebApp.Middlewares;
using StackExchange.Redis;

namespace ShivaEnterpriseWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            ConfigureCommonServices(services);

            var redis = ConnectionMultiplexer.Connect(Configuration.GetSection("RedisKeyStore").GetValue<string>("Endpoint"));

            //Services only for production
            //Shared Auth JWT token Cookie Sharing Code
            services.AddDataProtection().PersistKeysToStackExchangeRedis(redis, Configuration.GetSection("RedisKeyStore").GetValue<string>("AppKey"))
                .SetApplicationName(Configuration.GetSection("RedisKeyStore").GetValue<string>("AppName"));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = Configuration.GetSection("RedisKeyStore").GetValue<string>("CookieAppName");
                    //options.LoginPath = "/Auth/Index";
                    options.SlidingExpiration = true;
                    options.Cookie.Domain = Configuration.GetSection("RedisKeyStore").GetValue<string>("Domain");
                    options.Cookie.Path = "/";
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.HttpContext.Response.Redirect(Configuration.GetSection("RedisKeyStore").GetValue<string>("LoginPageUrl"));
                        return Task.CompletedTask;
                    };

                });


            //END :: Shared Auth JWT token Cookie Sharing Code
        }


        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            ConfigureCommonServices(services);

            //Services only for development
            //Shared Auth JWT token Cookie Sharing Code
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "ShivaWebCookie";
                    options.LoginPath = "/Login/Index";
                    options.SlidingExpiration = true;
                });

            //END :: Shared Auth JWT token Cookie Sharing Code
        }

        private void ConfigureCommonServices(IServiceCollection services)
        {
            //Services common to each environment

            services.AddControllersWithViews();

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

         

            services
                .AddRazorPages()
                .AddViewLocalization();

            services.AddScoped<RequestLocalizationCookiesMiddleware>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRequestLocalization();

            // will remember to write the cookie 
            app.UseRequestLocalizationCookies();

            app.UseRouting();

            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
                HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.None,
            };
            app.UseCookiePolicy(cookiePolicyOptions);
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });

          
        }
    }
}
