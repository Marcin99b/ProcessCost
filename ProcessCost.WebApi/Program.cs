using System.Reflection;
using ProcessCost.Database;
using ProcessCost.Database.Repositories;
using ProcessCost.Domain;
using ProcessCost.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<DatabaseContext>();
builder.Services.AddScoped<IStagesRepository, StagesRepository>();

var assemblies = typeof(Program)
    .Assembly
    .GetReferencedAssemblies()
    .Select(Assembly.Load)
    .ToArray();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(assemblies));

var app = builder.Build();

InitializeDb(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.SetupStagesApi();

app.Run();
return;

void InitializeDb(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetService<DatabaseContext>()!;
    context.Stages.AddRange([
        new() { Id = Guid.NewGuid(), Day = 01, MoneyAmount = 10_00, MoneyCurrency = "PLN", Name = "A", },
        new() { Id = Guid.NewGuid(), Day = 05, MoneyAmount = 10_00, MoneyCurrency = "PLN", Name = "A", },
        new() { Id = Guid.NewGuid(), Day = 10, MoneyAmount = 100_00, MoneyCurrency = "PLN", Name = "A", },
        new() { Id = Guid.NewGuid(), Day = 12, MoneyAmount = -80_00, MoneyCurrency = "PLN", Name = "A", },
        new() { Id = Guid.NewGuid(), Day = 12, MoneyAmount = 50_00, MoneyCurrency = "PLN", Name = "A", },
        new() { Id = Guid.NewGuid(), Day = 15, MoneyAmount = 200_00, MoneyCurrency = "PLN", Name = "A", },
        new() { Id = Guid.NewGuid(), Day = 16, MoneyAmount = -30_00, MoneyCurrency = "PLN", Name = "A", },
        new() { Id = Guid.NewGuid(), Day = 19, MoneyAmount = -5_00, MoneyCurrency = "PLN", Name = "A", },
        new() { Id = Guid.NewGuid(), Day = 21, MoneyAmount = 10_00, MoneyCurrency = "PLN", Name = "A", },
    ]);
    context.SaveChanges();
}

public partial class Program
{
}