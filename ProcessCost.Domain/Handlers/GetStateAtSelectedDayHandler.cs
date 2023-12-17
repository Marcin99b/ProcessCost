using MediatR;
using ProcessCost.Domain.Models;

namespace ProcessCost.Domain.Handlers;

public record GetStateAtSelectedDayRequest(int Day) : IRequest<GetStateAtSelectedDayResponse>;

public record GetStateAtSelectedDayResponse(Money Balance);

public class GetStateAtSelectedDayHandler : IRequestHandler<GetStateAtSelectedDayRequest, GetStateAtSelectedDayResponse>
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

    public Task<GetStateAtSelectedDayResponse> Handle(GetStateAtSelectedDayRequest request,
        CancellationToken cancellationToken)
    {
        var result = this._db
            .Where(x => x.Day <= request.Day)
            .Aggregate((a, b) => a.Add(b));

        return Task.FromResult(new GetStateAtSelectedDayResponse(result.Money));
    }
}