using ProcessCost.Database;
using ProcessCost.Database.Repositories;
using ProcessCost.Domain;
using ProcessCost.Domain.Models;
using ProcessCost.WebApi;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<DatabaseContext>();
builder.Services.AddScoped<IStagesRepository, StagesRepository>();
builder.Services.AddScoped<IStagesGroupsRepository, StagesGroupsRepository>();

var assemblies = typeof(Program)
    .Assembly
    .GetReferencedAssemblies()
    .Select(Assembly.Load)
    .ToArray();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(assemblies));

var app = builder.Build();

await InitializeDb(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.SetupStagesApi();

app.Run();
return;

static async Task InitializeDb(IHost app)
{
    List<Stage> stages =
    [
        new("A", 01, new(10M, Currency.PLN)),
        new("A", 05, new(10M, Currency.PLN)),
        new("A", 10, new(100M, Currency.PLN)),
        new("A", 12, new(-80M, Currency.PLN)),
        new("A", 12, new(50M, Currency.PLN)),
        new("A", 15, new(200M, Currency.PLN)),
        new("A", 16, new(-30M, Currency.PLN)),
        new("A", 19, new(-5M, Currency.PLN)),
        new("A", 21, new(10M, Currency.PLN)),
    ];

    var group = new StageGroup("ABC");
    group.AddStage(stages[0]);
    group.AddStage(stages[1]);

    using var scope = app.Services.CreateScope();
    var stagesRepository = scope.ServiceProvider.GetService<IStagesRepository>()!;
    var stagesGroupsRepository = scope.ServiceProvider.GetService<IStagesGroupsRepository>()!;

    foreach (var stage in stages)
    {
        await stagesRepository.Add(stage);
    }

    await stagesGroupsRepository.Add(group);
}

public partial class Program
{
}