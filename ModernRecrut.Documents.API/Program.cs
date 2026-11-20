using Microsoft.Extensions.Logging.Console;
using Microsoft.OpenApi.Models;
using ModernRecrut.Documents.API.Interfaces;
using ModernRecrut.Documents.API.Services;
using System.Reflection;

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
builder.Services.AddScoped<IDocumentService, DocumentService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Gestion des Documents",
        Version = "v1",
        Description = "API qui permet de gérer les documents d'un utilisateur",
        License = new OpenApiLicense
        {
            Name = "Apache 2.0",
            Url = new Uri("http://www.apache.org")
        },
        Contact = new OpenApiContact
        {
            Name = "Mamadou Diallo et Yvan Ngantchou",
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
