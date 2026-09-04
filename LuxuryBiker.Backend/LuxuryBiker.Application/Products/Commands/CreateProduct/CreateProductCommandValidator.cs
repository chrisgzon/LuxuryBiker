using FluentValidation;

namespace LuxuryBiker.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.CreateProductDto).NotNull();

            When(x => x.CreateProductDto is not null, () =>
            {
                RuleFor(x => x.CreateProductDto.Name)
                    .NotEmpty()
                    .MaximumLength(60);

                RuleFor(x => x.CreateProductDto.Reference)
                    .NotEmpty()
                    .MaximumLength(60);

                RuleFor(x => x.CreateProductDto.Description)
                    .MaximumLength(500);
            });
        }
    }
}
