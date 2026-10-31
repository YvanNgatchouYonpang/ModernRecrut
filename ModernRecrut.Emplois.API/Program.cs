using ModernRecrut.Emplois.API.Data;
using Microsoft.EntityFrameworkCore;
using ModernRecrut.Emplois.API.Interfaces;
using ModernRecrut.Emplois.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EmploiDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("EmploisConnection")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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
