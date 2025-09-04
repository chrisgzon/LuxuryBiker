import { ProductsRepository } from '@domain/products/repositories/products-repository';
import { ProductCreateUseCase } from '@domain/products/useCases/create.useCase';
import { ProductsImplementationRepository } from './products-implementation.repository';

const ProductCreateUseCaseFactory = 
(productsRepository: ProductsRepository) => new ProductCreateUseCase(productsRepository);
export const productCreateUseCaseProvider = {
    provide: ProductCreateUseCase,
    useFactory: ProductCreateUseCaseFactory,
    deps: [ProductsRepository],
};

export const productsImplementationRepositoryProvider = { provide: ProductsRepository, useClass: ProductsImplementationRepository }