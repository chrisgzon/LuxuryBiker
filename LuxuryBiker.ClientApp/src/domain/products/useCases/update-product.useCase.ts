import { UseCase } from '@base/use-case';
import { Observable } from 'rxjs';
import { ProductModel } from '../models/product.model';
import { ProductsRepository } from '../repositories/products-repository';

export class UpdateProductUseCase implements UseCase<ProductModel, void> {
  constructor(private productsRepository: ProductsRepository) {}

  execute(params: ProductModel): Observable<void> {
    return this.productsRepository.update(params);
  }
}
