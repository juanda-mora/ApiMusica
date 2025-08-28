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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ApiMusica",
        Version = "v1"
    });
});

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

// Mapear peticiones que **NO** son para /api ni /swagger para servir frontend
app.MapWhen(context =>
    !context.Request.Path.StartsWithSegments("/api") &&
    !context.Request.Path.StartsWithSegments("/swagger"),
    builder =>
    {
        builder.UseDefaultFiles();  // Sirve index.html por defecto
        builder.UseStaticFiles();   // Sirve js, css, imágenes

        builder.Run(async context =>
        {
            context.Response.ContentType = "text/html";
            await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath, "index.html"));
        });
    });

// Mapeo de controladores API
app.UseAuthorization();
app.MapControllers();

// Aplicar migraciones automáticamente al arrancar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MusicaContext>();
    db.Database.Migrate();
}

app.Run();
