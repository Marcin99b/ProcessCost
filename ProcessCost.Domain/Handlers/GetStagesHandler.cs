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

        public GetStagesHandler(Db db)
        {
            var item = new Stage("A", 3, new Money(2M, Currency.PLN));
            db.Stages.Add(item);
        }

        public Task<GetStagesResponse> Handle(GetStagesRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new GetStagesResponse(this._db));
        }
    }
}
