using LuxuryBiker.Application.Common.Interfaces.Services;
using LuxuryBiker.Domain.Constants;
using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Repositories.Products;

namespace LuxuryBiker.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public bool Status { get; set; }
        public string? Description { get; set; }
    }

    [Authorize(Roles = $"{Roles.Administrator}, {Roles.Seller}")]
    public record UpdateProductCommand(UpdateProductDto Dto) : IRequest<ErrorOr<Success>>;

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ErrorOr<Success>>
    {
        private readonly IProductsRepository _repository;
        private readonly IUser _user;

        public UpdateProductCommandHandler(IProductsRepository repository, IUser user)
        {
            _repository = repository;
            _user = user;
        }

        public async Task<ErrorOr<Success>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            UpdateProductDto dto = request.Dto;

            Product? product = await _repository.GetAsync(dto.Id);
            if (product is null)
                return ProductsErrors.NotFound;

            Product? sameReference = await _repository.GetByReference(dto.Reference);
            if (sameReference is not null && sameReference.Id != dto.Id)
                return ProductsErrors.Exists;

            product.UpdateDetails(dto.Name, dto.Reference, dto.Description, dto.Status);
            product.LastModifiedBy = _user.Id;

            await _repository.UpdateAsync(product);

            return Result.Success;
        }
    }
}
