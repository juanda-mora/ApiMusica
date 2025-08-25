using ApiMusica.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders; // Para archivos estáticos personalizados si hiciera falta
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MusicaContext>(options =>
    options.UseSqlite("Data Source=musica.db"));

// Aquí es donde agregamos el soporte para ciclos (para evitar problemas con referencias circulares)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

// Habilitar Swagger (documentación API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Habilitar CORS para permitir peticiones desde cualquier origen (útil para desarrollo)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Middleware pipeline

// Usar Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilitar CORS con la política "AllowAll"
app.UseCors("AllowAll");

// **Sirve archivos estáticos desde wwwroot**
app.UseDefaultFiles();  // Permite servir index.html por defecto
app.UseStaticFiles();   // Permite servir archivos estáticos (js, css, imágenes) desde wwwroot

app.MapFallbackToFile("index.html");  // Esto fuerza que se cargue index.html en la raíz

app.UseAuthorization();

app.MapControllers();

app.Run();
