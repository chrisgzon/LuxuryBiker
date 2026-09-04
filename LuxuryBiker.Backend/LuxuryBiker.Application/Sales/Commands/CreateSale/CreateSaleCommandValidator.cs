using FluentValidation;

namespace LuxuryBiker.Application.Sales.Commands.CreateSale
{
    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull();

            When(x => x.Dto is not null, () =>
            {
                RuleFor(x => x.Dto.Details)
                    .NotEmpty().WithMessage("La venta debe incluir al menos un producto.");

                RuleForEach(x => x.Dto.Details).ChildRules(detail =>
                {
                    detail.RuleFor(d => d.ProductId).GreaterThan(0);
                    detail.RuleFor(d => d.Quantity).GreaterThan(0);
                    detail.RuleFor(d => d.ProductValue).GreaterThan(0);
                });

                RuleFor(x => x.Dto.ThirdId)
                    .GreaterThan(0)
                    .When(x => x.Dto.ThirdId.HasValue);
            });
        }
    }
}
