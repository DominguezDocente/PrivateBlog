using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using PrivateBlog.Application;
using PrivateBlog.Persistence;
using PrivateBlog.Persistence.Seeding;
using PrivateBlog.Web.Middlewares;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 10;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
});

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices();

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    SeedDb service = scope.ServiceProvider.GetRequiredService<SeedDb>();
    await service.SeedAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.UseNotyf();
app.UseExceptionHandlerMiddleware();

app.Run();
