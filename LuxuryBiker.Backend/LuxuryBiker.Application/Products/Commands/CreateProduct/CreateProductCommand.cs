using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IUser _user;
        public CreateProductCommandHandler(IProductsRepository repository, IMapper mapper, IUser user)
        {
            _repository = repository;
            _mapper = mapper;
            _user = user;
        }
        public async Task<ErrorOr<string>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            bool exists = await this.ValidateProductExists(request.CreateProductDto.Reference);
            if (exists)
                return ProductsErrors.Exists;

            Product entity = _mapper.Map<Product>(request.CreateProductDto);
            entity.CreatedBy = _user.Id;
            entity.LastModifiedBy = _user.Id;
            entity.SetInternalCode();
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
