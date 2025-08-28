using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Data;
using Microsoft.Win32;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar dependencias
builder.Services.AddScoped<ICamisa, CamisaRepositorio>();
builder.Services.AddScoped<IMarca, MarcaRepositorio>();
builder.Services.AddScoped<IEstante, EstanteRepositorio>();
builder.Services.AddScoped<IVenta, VentaRepositorio>();

builder.Services.AddScoped<IAuth, AuthRepositorio>();
builder.Services.AddScoped<IUsuario, UsuarioRepositorio>();


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
