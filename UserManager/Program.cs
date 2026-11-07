using System.Text.Json.Serialization;
using FluentValidation;
using UserManager.Data;
using UserManager.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<UserStore>();
builder.Services.AddValidatorsFromAssemblyContaining<UserCreateValidator>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();

app.Run();
