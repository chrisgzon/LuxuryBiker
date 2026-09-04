import { UseCase } from '@base/use-case';
import { Observable } from 'rxjs';
import { ProductModel } from '../models/product.model';
import { ProductsRepository } from '../repositories/products-repository';

export class GetProductByIdUseCase implements UseCase<number, ProductModel> {
  constructor(private productsRepository: ProductsRepository) {}

  execute(id: number): Observable<ProductModel> {
    return this.productsRepository.getById(id);
  }
}
