using FluentValidation;

namespace LuxuryBiker.Application.Thirds.Queries.GetThirds
{
    public class GetThirdsQueryValidator : AbstractValidator<GetThirdsQuery>
    {
        public GetThirdsQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 200);
            RuleFor(x => x.TypeId).GreaterThan(0).When(x => x.TypeId.HasValue);
        }
    }
}
