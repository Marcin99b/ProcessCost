using System.Reflection;
using ProcessCost.Database;
using ProcessCost.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DatabaseContext>();

var assemblies = typeof(Program)
    .Assembly
    .GetReferencedAssemblies()
    .Select(Assembly.Load)
    .ToArray();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(assemblies));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.SetupStagesApi();

app.Run();

public partial class Program
{
}