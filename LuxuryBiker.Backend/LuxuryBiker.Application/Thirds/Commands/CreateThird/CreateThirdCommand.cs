using AutoMapper;
using LuxuryBiker.Application.Common.Interfaces.Services;
using LuxuryBiker.Domain.Constants;
using LuxuryBiker.Domain.Entities.Thirds;
using LuxuryBiker.Domain.Repositories.Thirds;
using MediatR;

namespace LuxuryBiker.Application.Thirds.Commands.CreateThird
{
    [Authorize(Roles = $"{Roles.Administrator}, {Roles.Sealer}")]
    public record CreateThirdCommand(ThirdDto CreateThirdDto) : IRequest<ErrorOr<int>>;
    internal class CreateThirdCommandhandler : IRequestHandler<CreateThirdCommand, ErrorOr<int>>
    {
        private readonly IThirdRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUser _user;
        public CreateThirdCommandhandler(IThirdRepository repository, IMapper mapper, IUser user)
        {
            _repository = repository;
            _mapper = mapper;
            _user = user;
        }
        public async Task<ErrorOr<int>> Handle(CreateThirdCommand request, CancellationToken cancellationToken)
        {
            bool existThird = await this.ValidateThirdExists(request.CreateThirdDto.Identification, request.CreateThirdDto.TypeId);
            if (existThird)
                return ThirdsErrors.Exists;

            Third entity = _mapper.Map<Third>(request.CreateThirdDto);
            entity.CreatedBy = _user.Id;
            await _repository.CreateAsync(entity);
            return entity.Id;
        }

        private async Task<bool> ValidateThirdExists(string? identification, int typeID)
        {
            if (string.IsNullOrWhiteSpace(identification))
                return false;

            Third? thirdExists = await _repository.GetByIdentification(identification);
            return thirdExists != null && thirdExists.TypeId.Equals(typeID);
        }
    }
}
