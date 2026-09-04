using FluentValidation;

namespace LuxuryBiker.Application.Thirds.Commands.CreateThird
{
    public class CreateThirdCommandValidator : AbstractValidator<CreateThirdCommand>
    {
        public CreateThirdCommandValidator()
        {
            RuleFor(x => x.CreateThirdDto).NotNull();

            When(x => x.CreateThirdDto is not null, () =>
            {
                RuleFor(x => x.CreateThirdDto.Identification)
                    .NotEmpty()
                    .MaximumLength(60);

                RuleFor(x => x.CreateThirdDto.Name)
                    .NotEmpty()
                    .MaximumLength(250);

                RuleFor(x => x.CreateThirdDto.Surnames)
                    .MaximumLength(250);

                RuleFor(x => x.CreateThirdDto.CellPhone)
                    .MaximumLength(50);

                RuleFor(x => x.CreateThirdDto.Address)
                    .MaximumLength(200);

                RuleFor(x => x.CreateThirdDto.Email)
                    .EmailAddress()
                    .When(x => !string.IsNullOrWhiteSpace(x.CreateThirdDto.Email));

                RuleFor(x => x.CreateThirdDto.TypeId)
                    .GreaterThan(0);
            });
        }
    }
}
