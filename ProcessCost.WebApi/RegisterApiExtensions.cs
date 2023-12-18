using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using ProcessCost.Domain.Handlers;
using Swashbuckle.AspNetCore.Annotations;

namespace ProcessCost.WebApi;

public static class RegisterApiExtensions
{
    private const string GroupStages = "Stages";
    private const string GroupCalculations = "Calculations";
    private const string GroupStagesGroups = "Stages Groups";

    public static WebApplication SetupStagesApi(this WebApplication app)
    {
        app.MapGet(
            "/stages",
            ([FromServices] IMediator mediator) =>
                mediator.Send(new GetStagesRequest()))
            .WithMetadata(new SwaggerOperationAttribute(description: "Get list of all stage"))
            .WithTags(GroupStages);

        app.MapGet(
            "/state/{day:int}",
            ([FromServices] IMediator mediator, [FromQuery] int day) =>
                mediator.Send(new GetStateAtSelectedDayRequest(day)))
            .WithMetadata(new SwaggerOperationAttribute(description: "Get money balance of selected day"))
            .WithTags(GroupCalculations);

        app.MapPost(
            "/stages/groups",
            ([FromServices] IMediator mediator, [FromBody] CreateStageGroupRequest request) =>
                mediator.Send(request))
            .WithMetadata(new SwaggerOperationAttribute(description: "Create new empty group"))
            .WithTags(GroupStagesGroups);

        app.MapDelete(
            "/stages/groups",
            ([FromServices] IMediator mediator, [FromBody] DeleteStageGroupRequest request) =>
                mediator.Send(request))
            .WithMetadata(new SwaggerOperationAttribute(description: "Delete group with all references"))
            .WithTags(GroupStagesGroups);

        app.MapPost(
            "/stages/groups/add",
            ([FromServices] IMediator mediator, [FromBody] AddStageToGroupRequest request) =>
                mediator.Send(request))
            .WithMetadata(new SwaggerOperationAttribute(description: "Add selected stage to selected group"))
            .WithTags(GroupStagesGroups);

        app.MapPost(
            "/stages/groups/remove",
            ([FromServices] IMediator mediator, [FromBody] RemoveStageFromGroupRequest request) =>
                mediator.Send(request))
            .WithMetadata(new SwaggerOperationAttribute(description: "Remove selected stage from selected group"))
            .WithTags(GroupStagesGroups);

        return app;
    }
}