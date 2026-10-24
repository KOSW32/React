using System.Reflection;
using Microsoft.OpenApi.Models;
using ShopApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DataStore>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Shop API",
        Version = "v1",
        Description = "REST API для роботи з товарами, замовленнями та позиціями замовлень.",
        Contact = new OpenApiContact
        {
            Name = "Студент",
            Email = "student@example.com"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop API v1");
});

app.UseRouting();
app.MapControllers();

app.Run();
