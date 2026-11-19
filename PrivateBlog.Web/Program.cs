using PrivateBlog.Web;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.AddCustomConfiguration();

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    endpoints.MapGet("/api/health", () => 
    {
        return "API is Working!";
    });
});
app.AddCustomWebApplicationConfiguration();

app.Run();
