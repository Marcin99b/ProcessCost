using MediatR;
using ProcessCost.Domain.Models;

namespace ProcessCost.Domain.Handlers
{
    public record GetStagesRequest : IRequest<GetStagesResponse>;
    public record GetStagesResponse(Stage[] Stages);

    public class GetStagesHandler : IRequestHandler<GetStagesRequest, GetStagesResponse>
    {
        private readonly Stage[] _db =
        [
            new Stage(01, new Money(10M, Currency.PLN)),
            new Stage(05, new Money(10M, Currency.PLN)),
            new Stage(10, new Money(100M, Currency.PLN)),
            new Stage(12, new Money(-80M, Currency.PLN)),
            new Stage(12, new Money(50M, Currency.PLN)),
            new Stage(15, new Money(200M, Currency.PLN)),
            new Stage(16, new Money(-30M, Currency.PLN)),
            new Stage(19, new Money(-5M, Currency.PLN)),
            new Stage(21, new Money(10M, Currency.PLN)),
        ];

        public Task<GetStagesResponse> Handle(GetStagesRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new GetStagesResponse(this._db));
        }
    }
}
