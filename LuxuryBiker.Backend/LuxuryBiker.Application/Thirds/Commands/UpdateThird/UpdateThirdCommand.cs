using LuxuryBiker.Application.Common.Interfaces.Services;
using LuxuryBiker.Domain.Constants;
using LuxuryBiker.Domain.Entities.Thirds;
using LuxuryBiker.Domain.Repositories.Thirds;

namespace LuxuryBiker.Application.Thirds.Commands.UpdateThird
{
    public class UpdateThirdDto
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string Identification { get; set; } = string.Empty;
        public string? Address { get; set; }
        public bool? Active { get; set; }
        public string? CellPhone { get; set; }
        public string? Name { get; set; }
        public string? Surnames { get; set; }
        public int TypeId { get; set; }
    }

    [Authorize(Roles = $"{Roles.Administrator}, {Roles.Seller}")]
    public record UpdateThirdCommand(UpdateThirdDto Dto) : IRequest<ErrorOr<Success>>;

    public class UpdateThirdCommandHandler : IRequestHandler<UpdateThirdCommand, ErrorOr<Success>>
    {
        private readonly IThirdRepository _repository;
        private readonly IUser _user;

        public UpdateThirdCommandHandler(IThirdRepository repository, IUser user)
        {
            _repository = repository;
            _user = user;
        }

        public async Task<ErrorOr<Success>> Handle(UpdateThirdCommand request, CancellationToken cancellationToken)
        {
            UpdateThirdDto dto = request.Dto;

            Third? third = await _repository.GetAsync(dto.Id);
            if (third is null)
                return ThirdsErrors.NotFound;

            Third? sameIdentification = await _repository.GetByIdentification(dto.Identification, dto.TypeId);
            if (sameIdentification is not null && sameIdentification.Id != dto.Id)
                return ThirdsErrors.Exists;

            third.Update(dto.Email, dto.Identification, dto.Address, dto.Active,
                dto.CellPhone, dto.Name, dto.Surnames, dto.TypeId);
            third.LastModifiedBy = _user.Id;

            await _repository.UpdateAsync(third);

            return Result.Success;
        }
    }
}
