using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProcessCost.Domain.Handlers;

namespace ProcessCost.WebApi;

public static class RegisterApiExtensions
{
    public static WebApplication SetupStagesApi(this WebApplication app)
    {
        app.MapGet(
            "/stages",
            ([FromServices] IMediator mediator) =>
                mediator.Send(new GetStagesRequest()));

        app.MapGet(
            "/state/{day:int}",
            ([FromServices] IMediator mediator, int day) =>
                mediator.Send(new GetStateAtSelectedDayRequest(day)));

        app.MapPost(
            "/stages/groups",
            ([FromServices] IMediator mediator, [FromBody] CreateStageGroupRequest request) =>
                mediator.Send(request));

        app.MapPost(
            "/stages/groups/add",
            ([FromServices] IMediator mediator, [FromBody] AddStageToGroupRequest request) =>
                mediator.Send(request));

        app.MapPost(
            "/stages/groups/remove",
            ([FromServices] IMediator mediator, [FromBody] RemoveStageFromGroupRequest request) =>
                mediator.Send(request));

        return app;
    }
}