using AutoMapper;
using LuxuryBiker.Application.Common.Models;
using LuxuryBiker.Domain.Entities.Thirds;
using LuxuryBiker.Domain.Repositories.Thirds;

namespace LuxuryBiker.Application.Thirds.Queries.GetThirds
{
    [Authorize]
    public record GetThirdsQuery(int PageNumber = 1, int PageSize = 20, int? TypeId = null)
        : IRequest<ErrorOr<PaginatedList<ThirdBriefDto>>>;

    public class GetThirdsQueryHandler
        : IRequestHandler<GetThirdsQuery, ErrorOr<PaginatedList<ThirdBriefDto>>>
    {
        private readonly IThirdRepository _repository;
        private readonly IMapper _mapper;

        public GetThirdsQueryHandler(IThirdRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<PaginatedList<ThirdBriefDto>>> Handle(
            GetThirdsQuery request, CancellationToken cancellationToken)
        {
            (IReadOnlyList<Third> items, int totalCount) = await _repository.GetPagedAsync(
                request.PageNumber, request.PageSize, request.TypeId, cancellationToken);

            var dtos = _mapper.Map<IReadOnlyList<ThirdBriefDto>>(items);

            return new PaginatedList<ThirdBriefDto>(dtos, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
