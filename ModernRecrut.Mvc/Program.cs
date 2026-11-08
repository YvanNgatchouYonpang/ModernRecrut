using Microsoft.Extensions.Logging.Console;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration des niveaux de journalisation
builder.Logging.AddFilter<ConsoleLoggerProvider>(
    "Microsoft.AspNetCore",
    LogLevel.Warning
);



builder.Logging.ClearProviders();

builder.Logging.AddConsole();

builder.Logging.AddEventLog();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<IEmploisService, EmploisServiceProxy>(
    client =>
    {
        client.BaseAddress =
            new Uri(builder.Configuration.GetValue<string>("UrlApiEmplois"));
    });

builder.Services.AddHttpClient<IFavorisService, FavorisServiceProxy>(
    client =>
    {
        client.BaseAddress =
            new Uri(builder.Configuration.GetValue<string>("UrlApiFavoris"));
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}
app.UseExceptionHandler("/Errors");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseStatusCodePagesWithRedirects("/Errors/HttpError?statusCode={0}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
