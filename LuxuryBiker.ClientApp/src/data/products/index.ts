import { ProductsRepository } from '@domain/products/repositories/products-repository';
import { ProductCreateUseCase } from '@domain/products/useCases/create.useCase';
import { GetProductsUseCase } from '@domain/products/useCases/get-products.useCase';
import { GetProductByIdUseCase } from '@domain/products/useCases/get-product-by-id.useCase';
import { UpdateProductUseCase } from '@domain/products/useCases/update-product.useCase';
import { ProductsImplementationRepository } from './products-implementation.repository';

const ProductCreateUseCaseFactory =
(productsRepository: ProductsRepository) => new ProductCreateUseCase(productsRepository);
export const productCreateUseCaseProvider = {
    provide: ProductCreateUseCase,
    useFactory: ProductCreateUseCaseFactory,
    deps: [ProductsRepository],
};

const GetProductsUseCaseFactory =
(productsRepository: ProductsRepository) => new GetProductsUseCase(productsRepository);
export const getProductsUseCaseProvider = {
    provide: GetProductsUseCase,
    useFactory: GetProductsUseCaseFactory,
    deps: [ProductsRepository],
};

const GetProductByIdUseCaseFactory =
(productsRepository: ProductsRepository) => new GetProductByIdUseCase(productsRepository);
export const getProductByIdUseCaseProvider = {
    provide: GetProductByIdUseCase,
    useFactory: GetProductByIdUseCaseFactory,
    deps: [ProductsRepository],
};

const UpdateProductUseCaseFactory =
(productsRepository: ProductsRepository) => new UpdateProductUseCase(productsRepository);
export const updateProductUseCaseProvider = {
    provide: UpdateProductUseCase,
    useFactory: UpdateProductUseCaseFactory,
    deps: [ProductsRepository],
};

export const productsImplementationRepositoryProvider = { provide: ProductsRepository, useClass: ProductsImplementationRepository }
