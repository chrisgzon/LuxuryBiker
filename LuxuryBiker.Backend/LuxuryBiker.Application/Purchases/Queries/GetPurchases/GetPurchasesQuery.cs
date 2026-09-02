using AutoMapper;
using LuxuryBiker.Application.Common.Models;
using LuxuryBiker.Domain.Entities.Purchases;
using LuxuryBiker.Domain.Repositories.Purchases;

namespace LuxuryBiker.Application.Purchases.Queries.GetPurchases
{
    [Authorize]
    public record GetPurchasesQuery(
        int PageNumber = 1,
        int PageSize = 20,
        DateTimeOffset? DateFrom = null,
        DateTimeOffset? DateTo = null) : IRequest<ErrorOr<PaginatedList<PurchaseBriefDto>>>;

    public class GetPurchasesQueryHandler
        : IRequestHandler<GetPurchasesQuery, ErrorOr<PaginatedList<PurchaseBriefDto>>>
    {
        private readonly IPurchasesRepository _repository;
        private readonly IMapper _mapper;

        public GetPurchasesQueryHandler(IPurchasesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<PaginatedList<PurchaseBriefDto>>> Handle(
            GetPurchasesQuery request, CancellationToken cancellationToken)
        {
            (IReadOnlyList<Purchase> items, int totalCount) = await _repository.GetPagedAsync(
                request.PageNumber, request.PageSize, request.DateFrom, request.DateTo, cancellationToken);

            var dtos = _mapper.Map<IReadOnlyList<PurchaseBriefDto>>(items);

            return new PaginatedList<PurchaseBriefDto>(dtos, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
