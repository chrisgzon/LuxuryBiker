using AutoMapper;
using LuxuryBiker.Application.Common.Models;
using LuxuryBiker.Domain.Entities.Sales;
using LuxuryBiker.Domain.Repositories.Sales;

namespace LuxuryBiker.Application.Sales.Queries.GetSales
{
    [Authorize]
    public record GetSalesQuery(
        int PageNumber = 1,
        int PageSize = 20,
        DateTimeOffset? DateFrom = null,
        DateTimeOffset? DateTo = null) : IRequest<ErrorOr<PaginatedList<SaleBriefDto>>>;

    public class GetSalesQueryHandler
        : IRequestHandler<GetSalesQuery, ErrorOr<PaginatedList<SaleBriefDto>>>
    {
        private readonly ISalesRepository _repository;
        private readonly IMapper _mapper;

        public GetSalesQueryHandler(ISalesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<PaginatedList<SaleBriefDto>>> Handle(
            GetSalesQuery request, CancellationToken cancellationToken)
        {
            (IReadOnlyList<Sale> items, int totalCount) = await _repository.GetPagedAsync(
                request.PageNumber, request.PageSize, request.DateFrom, request.DateTo, cancellationToken);

            var dtos = _mapper.Map<IReadOnlyList<SaleBriefDto>>(items);

            return new PaginatedList<SaleBriefDto>(dtos, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
