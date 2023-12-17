using MediatR;
using ProcessCost.Domain.Models;

namespace ProcessCost.Domain.Handlers;

public record GetStateAtSelectedDayRequest(int Day) : IRequest<GetStateAtSelectedDayResponse>;

public record GetStateAtSelectedDayResponse(Money Balance);

public class GetStateAtSelectedDayHandler(IStagesRepository stagesRepository)
    : IRequestHandler<GetStateAtSelectedDayRequest, GetStateAtSelectedDayResponse>
{
    public async Task<GetStateAtSelectedDayResponse> Handle(GetStateAtSelectedDayRequest request,
        CancellationToken cancellationToken)
    {
        var stages = await stagesRepository.GetAllStagesOfUser(Guid.Empty);
        var result = stages
            .Where(x => x.Day <= request.Day)
            .Aggregate((a, b) => a.Add(b));

        return new GetStateAtSelectedDayResponse(result.Money);
    }
}