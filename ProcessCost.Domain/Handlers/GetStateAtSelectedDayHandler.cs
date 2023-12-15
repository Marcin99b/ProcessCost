using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ProcessCost.Domain.Models;

namespace ProcessCost.Domain.Handlers
{
    public record GetStateAtSelectedDayRequest(int Day) : IRequest<GetStateAtSelectedDayResponse>;
    public record GetStateAtSelectedDayResponse(Money Balance);

    public class GetStateAtSelectedDayHandler : IRequestHandler<GetStateAtSelectedDayRequest, GetStateAtSelectedDayResponse>
    {
        private readonly Stage[] _db =
        [
            new Stage("A", 01, new Money(10M, Currency.PLN)),
            new Stage("A", 05, new Money(10M, Currency.PLN)),
            new Stage("A", 10, new Money(100M, Currency.PLN)),
            new Stage("A", 12, new Money(-80M, Currency.PLN)),
            new Stage("A", 12, new Money(50M, Currency.PLN)),
            new Stage("A", 15, new Money(200M, Currency.PLN)),
            new Stage("A", 16, new Money(-30M, Currency.PLN)),
            new Stage("A", 19, new Money(-5M, Currency.PLN)),
            new Stage("A", 21, new Money(10M, Currency.PLN)),
        ];

        public Task<GetStateAtSelectedDayResponse> Handle(GetStateAtSelectedDayRequest request, CancellationToken cancellationToken)
        {
            var result = this._db
                .Where(x => x.Day <= request.Day)
                .Aggregate((a,b) => a.Add(b));

            return Task.FromResult(new GetStateAtSelectedDayResponse(result.Money));
        }
    }
}
