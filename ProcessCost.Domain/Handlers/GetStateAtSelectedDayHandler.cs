using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ProcessCost.Domain.Handlers
{
    public record GetStateAtSelectedDayRequest : IRequest<GetStateAtSelectedDayResponse>;
    public record GetStateAtSelectedDayResponse;

    public class GetStateAtSelectedDayHandler : IRequestHandler<GetStateAtSelectedDayRequest, GetStateAtSelectedDayResponse>
    {
        public Task<GetStateAtSelectedDayResponse> Handle(GetStateAtSelectedDayRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
