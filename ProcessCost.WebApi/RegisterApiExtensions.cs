using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcessCost.Database;
using ProcessCost.Database.Repositories;
using ProcessCost.Domain;
using ProcessCost.Domain.Handlers;
using Swashbuckle.AspNetCore.Annotations;

namespace ProcessCost.WebApi;

public static class RegisterApiExtensions
{
    private const string GroupStages = "Stages";
    private const string GroupCalculations = "Calculations";
    private const string GroupStagesGroups = "Stages Groups";

    private const string Version10 = "v1.0";

    public static WebApplication SetupStagesApiV1(this WebApplication app)
    {
        return app
            .GetStagesV1()
            .AddStageV1()
            .UpdateStageMoneyV1()
            .GetStateAtSelectedDayV1()
            .CreateStageGroupV1()
            .DeleteStageGroupV1()
            .AddStageToGroupV1()
            .RemoveStageFromGroupV1();
    }

    private static WebApplication GetStagesV1(this WebApplication app)
    {
        app.MapGet(
                $"/{Version10}/stages",
                ([FromServices] IMediator mediator) =>
                    mediator.Send(new GetStagesRequest()))
            .WithMetadata(new SwaggerOperationAttribute(description: "Get list of all stage"))
            .WithTags(GroupStages);
        return app;
    }

    private static WebApplication AddStageV1(this WebApplication app)
    {
        app.MapPost(
                $"/{Version10}/stages",
                ([FromServices] IMediator mediator, [FromBody] AddStageRequest request) =>
                    mediator.Send(request))
            .WithMetadata(new SwaggerOperationAttribute(description: "Create new stage"))
            .WithTags(GroupStages);
        return app;
    }

    private static WebApplication UpdateStageMoneyV1(this WebApplication app)
    {
        app.MapPost(
                $"/{Version10}/stages/money",
                ([FromServices] IMediator mediator, [FromBody] UpdateStageMoneyRequest request) =>
                    mediator.Send(request))
            .WithMetadata(new SwaggerOperationAttribute(description: "Update money in selected stage and dependencies"))
            .WithTags(GroupStages);
        return app;
    }

    private static WebApplication GetStateAtSelectedDayV1(this WebApplication app)
    {
        app.MapGet(
                $"/{Version10}/state/{{day:int}}",
                ([FromServices] IMediator mediator, int day) =>
                    mediator.Send(new GetStateAtSelectedDayRequest(day)))
            .WithMetadata(new SwaggerOperationAttribute(description: "Get money balance of selected day"))
            .WithTags(GroupCalculations);
        return app;
    }

    private static WebApplication CreateStageGroupV1(this WebApplication app)
    {
        app.MapPost(
                $"/{Version10}/stages/groups",
                ([FromServices] IMediator mediator, [FromBody] CreateStageGroupRequest request) =>
                    mediator.Send(request))
            .WithMetadata(new SwaggerOperationAttribute(description: "Create new empty group"))
            .WithTags(GroupStagesGroups);
        return app;
    }

    private static WebApplication DeleteStageGroupV1(this WebApplication app)
    {
        app.MapDelete(
                $"/{Version10}/stages/groups/{{groupId:guid}}",
                ([FromServices] IMediator mediator, Guid groupId) =>
                    mediator.Send(new DeleteStageGroupRequest(groupId)))
            .WithMetadata(new SwaggerOperationAttribute(description: "Delete group with all references"))
            .WithTags(GroupStagesGroups);
        return app;
    }

    private static WebApplication AddStageToGroupV1(this WebApplication app)
    {
        app.MapPost(
                $"/{Version10}/stages/groups/add",
                ([FromServices] IMediator mediator, [FromBody] AddStageToGroupRequest request) =>
                    mediator.Send(request))
            .WithMetadata(new SwaggerOperationAttribute(description: "Add selected stage to selected group"))
            .WithTags(GroupStagesGroups);
        return app;
    }

    private static WebApplication RemoveStageFromGroupV1(this WebApplication app)
    {
        app.MapPost(
                $"/{Version10}/stages/groups/remove",
                ([FromServices] IMediator mediator, [FromBody] RemoveStageFromGroupRequest request) =>
                    mediator.Send(request))
            .WithMetadata(new SwaggerOperationAttribute(description: "Remove selected stage from selected group"))
            .WithTags(GroupStagesGroups);
        return app;
    }

    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services
            .AddScoped(_ => new DatabaseContext(new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase("ProcessCostDb")
                .Options))
            .AddScoped<IStagesRepository, StagesRepository>()
            .AddScoped<IStagesGroupsRepository, StagesGroupsRepository>()
            .AddSingleton<IStagesEventBus, StagesEventBus>()
            .AddHostedService<EventBusExecutor>();

        return services;
    }

    public static IServiceCollection RegisterMediatr(this IServiceCollection services)
    {
        var assemblies = typeof(Program)
            .Assembly
            .GetReferencedAssemblies()
            .Select(Assembly.Load)
            .ToArray();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(assemblies));

        return services;
    }
}