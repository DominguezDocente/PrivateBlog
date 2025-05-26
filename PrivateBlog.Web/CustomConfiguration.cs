using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.Data.Seeders;
using PrivateBlog.Web.Helpers;
using PrivateBlog.Web.Services;
using Serilog;
using System.Text;

namespace PrivateBlog.Web
{
    public static class CustomConfiguration
    {
        public static WebApplicationBuilder AddCustomConfiguration(this WebApplicationBuilder builder)
        {
            // Data Context
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
            });

            builder.Services.AddHttpContextAccessor();

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            // Services
            AddServices(builder);

            // Identity and Acces Managment
            AddIAM(builder);

            // Toast Notification Setup
            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            // Log Setup
            AddLogConfiguration(builder);

            return builder;
        }

        private static void AddIAM(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<User, IdentityRole>(conf =>
            {
                conf.User.RequireUniqueEmail = true;
                conf.Password.RequireDigit = false;
                conf.Password.RequiredUniqueChars = 0;
                conf.Password.RequireLowercase = false;
                conf.Password.RequireUppercase = false;
                conf.Password.RequireNonAlphanumeric = false;
                conf.Password.RequiredLength = 4;
            }).AddEntityFrameworkStores<DataContext>()
              .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(conf =>
            {
                conf.Cookie.Name = "Auth";
                conf.ExpireTimeSpan = TimeSpan.FromDays(100);
                conf.LoginPath = "/Account/Login";
                conf.AccessDeniedPath = "/Errors/403";

                // ? Evita redirección en APIs (por ejemplo /api/...)
                conf.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api"))
                        {
                            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return Task.CompletedTask;
                        }

                        ctx.Response.Redirect(ctx.RedirectUri);
                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api"))
                        {
                            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                            return Task.CompletedTask;
                        }

                        ctx.Response.Redirect(ctx.RedirectUri);
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddAuthentication()
                            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                            {
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                                    ValidAudience = builder.Configuration["Jwt:Audience"],
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                                };
                            });
                            
        }

        private static void AddLogConfiguration(WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration().MinimumLevel.Warning()
                                                  .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                                                  .WriteTo.File("logs/log.log", rollingInterval: RollingInterval.Day)
                                                  .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Log.Fatal(args.ExceptionObject as Exception, "Excepción no manejada");
                Log.CloseAndFlush();
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                Log.Fatal(args.Exception, "Excepción no observada en Task");
                args.SetObserved();
                Log.CloseAndFlush();
            };
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            // Servicios
            builder.Services.AddScoped<IBlogsService, BlogsService>();
            builder.Services.AddScoped<IHomeService, HomeService>();
            builder.Services.AddScoped<ILogService, LocalLogSercive>();
            builder.Services.AddTransient<IEmailService, MailtrapService>();
            builder.Services.AddScoped<IRolesService, RolesService>();
            builder.Services.AddScoped<ISectionsService, SectionsService>();
            builder.Services.AddTransient<SeedDb>();
            builder.Services.AddTransient<IStorageService, LocalStorageService>();
            builder.Services.AddScoped<IUsersService, UsersService>();

            // Helpers
            builder.Services.AddTransient<ICombosHelper, CombosHelper>();
        }

        public static WebApplication AddcustomWebApplicationConfiguration(this WebApplication app)
        {
            app.UseNotyf();

            SeedData(app);

            return app;
        }

        private static void SeedData(WebApplication app)
        {
            IServiceScopeFactory scopeFactory = app.Services.GetService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory!.CreateScope())
            {
                SeedDb service = scope.ServiceProvider.GetService<SeedDb>();
                service!.SeedAsync().Wait();
            }
        }
    }
}
