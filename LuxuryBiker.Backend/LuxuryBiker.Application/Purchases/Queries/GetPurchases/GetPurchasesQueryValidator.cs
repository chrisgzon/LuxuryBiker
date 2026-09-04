using FluentValidation;

namespace LuxuryBiker.Application.Purchases.Queries.GetPurchases
{
    public class GetPurchasesQueryValidator : AbstractValidator<GetPurchasesQuery>
    {
        public GetPurchasesQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 200);
            RuleFor(x => x.DateTo)
                .GreaterThanOrEqualTo(x => x.DateFrom!.Value)
                .When(x => x.DateFrom.HasValue && x.DateTo.HasValue)
                .WithMessage("La fecha final debe ser posterior a la inicial.");
        }
    }
}
