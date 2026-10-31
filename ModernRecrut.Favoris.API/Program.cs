using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ModernRecrut.Favoris.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<FavorisService>();

// Configuration du cache mémoire
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 5000000;
});


// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient<OffreEmploiApiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5263/");
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
