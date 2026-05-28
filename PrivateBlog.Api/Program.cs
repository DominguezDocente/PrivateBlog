using PrivateBlog.Application;
using PrivateBlog.Persistence;
using PrivateBlog.Persistence.Seeding;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices();

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    SeedDb service = scope.ServiceProvider.GetRequiredService<SeedDb>();
    await service.SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
