using MediatR;
using ProcessCost.Domain.Models;

namespace ProcessCost.Domain.Handlers;

public record GetStagesRequest : IRequest<GetStagesResponse>;

public record GetStagesResponse(Stage[] Stages);

public class GetStagesHandler : IRequestHandler<GetStagesRequest, GetStagesResponse>
{
    private readonly Stage[] _db =
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

    public Task<GetStagesResponse> Handle(GetStagesRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new GetStagesResponse(this._db));
    }
}