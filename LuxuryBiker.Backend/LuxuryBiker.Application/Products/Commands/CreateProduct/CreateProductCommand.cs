using LuxuryBiker.Application.Common.Interfaces.Services;
using LuxuryBiker.Domain.Constants;
using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Repositories.Products;

namespace LuxuryBiker.Application.Products.Commands.CreateProduct
{
    [Authorize(Roles = $"{Roles.Administrator}, {Roles.Seller}")]
    public record CreateProductCommand(CreateProductDto CreateProductDto) : IRequest<ErrorOr<string>>;
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ErrorOr<string>>
    {
        private readonly IProductsRepository _repository;
        private readonly IUser _user;
        public CreateProductCommandHandler(IProductsRepository repository, IUser user)
        {
            _repository = repository;
            _user = user;
        }
        public async Task<ErrorOr<string>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            CreateProductDto dto = request.CreateProductDto;

            bool exists = await this.ValidateProductExists(dto.Reference);
            if (exists)
                return ProductsErrors.Exists;

            Product entity = Product.Create(dto.Name, dto.Reference, dto.Description, dto.Status);
            entity.CreatedBy = _user.Id;
            entity.LastModifiedBy = _user.Id;

            await _repository.CreateAsync(entity);
            return entity.Code!;
        }

        private async Task<bool> ValidateProductExists(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
                return false;

            Product? exists = await _repository.GetByReference(reference);
            return exists != null;
        }
    }
}
