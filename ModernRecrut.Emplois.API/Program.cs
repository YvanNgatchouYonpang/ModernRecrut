using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using Microsoft.OpenApi.Models;
using ModernRecrut.Emplois.API.Data;
using ModernRecrut.Emplois.API.Interfaces;
using ModernRecrut.Emplois.API.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configuration des niveaux de journalisation
builder.Logging.AddFilter<ConsoleLoggerProvider>(
    "Microsoft.AspNetCore",
    LogLevel.Warning
);

builder.Logging.AddFilter<ConsoleLoggerProvider>(
    "Microsoft.EntityFrameworkCore",
    LogLevel.Warning
);



builder.Services.AddDbContext<EmploiDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("EmploisConnection")));

// Add services to the container.

builder.Logging.ClearProviders();

builder.Logging.AddConsole();

builder.Logging.AddEventLog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Gestion des Emplois",
        Version = "v1",
        Description = "API qui permet de gérer les offres d'emploi de l'entreprise ModernRecrut",
        License = new OpenApiLicense
        {
            Name = "Apache 2.0",
            Url = new Uri("http://www.apache.org")
        },
        Contact = new OpenApiContact
        {
            Name = "Mamadou Diallo et Yvan Ngatchou",
            Email = "modern.it@modern-ti.com",
            Url = new Uri("https://modern-ti.com/")
        }
    });
    //Activation du support des commentaires XML dans Swagger UI
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
}

    ));
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEmploiService, EmploiService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
