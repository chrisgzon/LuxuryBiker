using FluentValidation;

namespace LuxuryBiker.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull();

            When(x => x.Dto is not null, () =>
            {
                RuleFor(x => x.Dto.Id).GreaterThan(0);
                RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(60);
                RuleFor(x => x.Dto.Reference).NotEmpty().MaximumLength(60);
                RuleFor(x => x.Dto.Description).MaximumLength(500);
            });
        }
    }
}
