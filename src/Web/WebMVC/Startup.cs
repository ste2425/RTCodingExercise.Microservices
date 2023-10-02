using System.Text;
using MassTransit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using RTCodingExercise.Microservices.Services;

namespace RTCodingExercise.WebMVC
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
            services.AddSession();
            services.AddControllers();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITokenService, TokenService>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddMassTransit(x =>
            {
                //x.AddConsumer<ConsumerClass>();

                //ADD CONSUMERS HERE
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["EventBusConnection"], "/", h =>
                    {
                        if (!string.IsNullOrEmpty(Configuration["EventBusUserName"]))
                        {
                            h.Username(Configuration["EventBusUserName"]);
                        }

                        if (!string.IsNullOrEmpty(Configuration["EventBusPassword"]))
                        {
                            h.Password(Configuration["EventBusPassword"]);
                        }
                    });

                    cfg.ConfigureEndpoints(context);
                    cfg.ExchangeType = ExchangeType.Fanout;
                });
            });

            services.AddMassTransitHostedService();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opts =>
                {
                    opts.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    opts.SlidingExpiration = true;
                    opts.LoginPath = "/login";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            app.UseSession();

            app.UseStaticFiles();
            app.UseForwardedHeaders();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });
        }
    }
}
