using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProcessCost.Domain.Handlers;

namespace ProcessCost.WebApi
{
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

            return app;
        }
    }
}