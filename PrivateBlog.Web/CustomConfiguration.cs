﻿using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Seeders;
using PrivateBlog.Web.Helpers;
using PrivateBlog.Web.Services;
using Serilog;
using System.Runtime.CompilerServices;

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

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            // Services
            AddServices(builder);

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

        private static void AddLogConfiguration(WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File("logs/log.log",
                                                                rollingInterval: RollingInterval.Day, 
                                                                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
                                                  .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                                                  .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            // Servicios
            builder.Services.AddScoped<IBlogsService, BlogsService>();
            builder.Services.AddScoped<IReadLogsService, ReadPlainTextLogsService>();
            builder.Services.AddScoped<ISectionsService, SectionsService>();
            builder.Services.AddTransient<SeedDb>();

            // Helpers
            builder.Services.AddScoped<ICombosHelper, CombosHelper>();
        }

        public static WebApplication AddcustomWebApplicationConfiguration(this WebApplication app)
        {
            app.UseNotyf();

            SeedData(app);

            return app;
        }

        private static void SeedData(WebApplication app)
        {
            IServiceScopeFactory scopeFactory = app.Services.GetService<IServiceScopeFactory>()!;

            using IServiceScope scope = scopeFactory!.CreateScope();
            SeedDb service = scope.ServiceProvider.GetService<SeedDb>()!;
            service!.SeedAsync().Wait();
        }
    }
}
