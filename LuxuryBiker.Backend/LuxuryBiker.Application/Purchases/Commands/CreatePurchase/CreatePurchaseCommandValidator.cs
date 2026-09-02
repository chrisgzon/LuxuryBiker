using FluentValidation;

namespace LuxuryBiker.Application.Purchases.Commands.CreatePurchase
{
    public class CreatePurchaseCommandValidator : AbstractValidator<CreatePurchaseCommand>
    {
        public CreatePurchaseCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull();

            When(x => x.Dto is not null, () =>
            {
                RuleFor(x => x.Dto.Details)
                    .NotEmpty().WithMessage("La compra debe incluir al menos un producto.");

                RuleForEach(x => x.Dto.Details).ChildRules(detail =>
                {
                    detail.RuleFor(d => d.ProductId).GreaterThan(0);
                    detail.RuleFor(d => d.Quantity).GreaterThan(0);
                    detail.RuleFor(d => d.ProductValue).GreaterThan(0);
                });

                RuleFor(x => x.Dto.DatePurchase)
                    .LessThanOrEqualTo(_ => DateTimeOffset.Now)
                    .WithMessage("La fecha de compra no puede ser futura.");

                RuleFor(x => x.Dto.ThirdId)
                    .GreaterThan(0)
                    .When(x => x.Dto.ThirdId.HasValue);
            });
        }
    }
}
