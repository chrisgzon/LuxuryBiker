using AutoMapper;
using LuxuryBiker.Application.Thirds.Queries.GetThirds;
using LuxuryBiker.Domain.Entities.Thirds;
using LuxuryBiker.Domain.Repositories.Thirds;

namespace LuxuryBiker.Application.Thirds.Queries.GetThirdById
{
    [Authorize]
    public record GetThirdByIdQuery(int Id) : IRequest<ErrorOr<ThirdBriefDto>>;

    public class GetThirdByIdQueryHandler : IRequestHandler<GetThirdByIdQuery, ErrorOr<ThirdBriefDto>>
    {
        private readonly IThirdRepository _repository;
        private readonly IMapper _mapper;

        public GetThirdByIdQueryHandler(IThirdRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<ThirdBriefDto>> Handle(GetThirdByIdQuery request, CancellationToken cancellationToken)
        {
            Third? third = await _repository.GetAsync(request.Id);
            if (third is null)
                return ThirdsErrors.NotFound;

            return _mapper.Map<ThirdBriefDto>(third);
        }
    }
}
