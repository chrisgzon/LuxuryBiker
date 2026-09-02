using FluentValidation;

namespace LuxuryBiker.Application.Thirds.Commands.UpdateThird
{
    public class UpdateThirdCommandValidator : AbstractValidator<UpdateThirdCommand>
    {
        public UpdateThirdCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull();

            When(x => x.Dto is not null, () =>
            {
                RuleFor(x => x.Dto.Id).GreaterThan(0);
                RuleFor(x => x.Dto.Identification).NotEmpty().MaximumLength(60);
                RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(250);
                RuleFor(x => x.Dto.Surnames).MaximumLength(250);
                RuleFor(x => x.Dto.CellPhone).MaximumLength(50);
                RuleFor(x => x.Dto.Address).MaximumLength(200);
                RuleFor(x => x.Dto.Email).EmailAddress()
                    .When(x => !string.IsNullOrWhiteSpace(x.Dto.Email));
                RuleFor(x => x.Dto.TypeId).GreaterThan(0);
            });
        }
    }
}
