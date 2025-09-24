using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using SherDashed.Services;

namespace SherDashed;

public class Program
{
    public static async Task Main(string[] args)
    { 
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
        });
        
        builder.Services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        builder.Services.AddSingleton<JsonDataService>();
        builder.Services.AddScoped<AnnouncementService>();
        
        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var announcementService = scope.ServiceProvider.GetRequiredService<AnnouncementService>();
            await announcementService.InitializeAsync();
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}