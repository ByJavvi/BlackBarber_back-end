using BlackBarberAPI.Data;
using BlackBarberAPI.Utilidades;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// 1. Solo necesitas AddSwaggerGen para la interfaz clásica
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<BlackBarberContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.InyectarDependencias(builder.Configuration);

//Configura los CORS y otros servicios
var origenesPermitidos = builder.Configuration.GetValue<string>("OrigenesPermitidos")!.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
if (origenesPermitidos.Length == 0)
    throw new InvalidOperationException("Falta 'OrigenesPermitidos' en configuración.");

builder.Services.AddCors(zOptions =>
{
    if (origenesPermitidos != null && origenesPermitidos.Length > 0)
    {
        zOptions.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins(origenesPermitidos)
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    }
    else
    {
        // Configuración de CORS por defecto si no hay AllowedHosts configurados
        zOptions.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    }
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // 2. Activa el generador y la UI
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Esto asegura que apunte al JSON correcto
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API V1");
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();