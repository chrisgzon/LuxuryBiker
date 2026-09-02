import { Injectable } from '@angular/core';
import { ProductModel } from '@domain/products/models/product.model';
import { ProductCreateUseCase } from '@domain/products/useCases/create.useCase';
import { GetProductsUseCase } from '@domain/products/useCases/get-products.useCase';
import { GetProductByIdUseCase } from '@domain/products/useCases/get-product-by-id.useCase';
import { UpdateProductUseCase } from '@domain/products/useCases/update-product.useCase';
import { GetProductsParams } from '@domain/products/repositories/products-repository';
import { PaginatedResult } from '@domain/common/models/paginated-result.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {

  constructor(
    private productCreateUseCase: ProductCreateUseCase,
    private getProductsUseCase: GetProductsUseCase,
    private getProductByIdUseCase: GetProductByIdUseCase,
    private updateProductUseCase: UpdateProductUseCase
  ) { }

  create(product: ProductModel): Observable<string> {
    return this.productCreateUseCase.execute(product);
  }

  getAll(params: GetProductsParams = {}): Observable<PaginatedResult<ProductModel>> {
    return this.getProductsUseCase.execute(params);
  }

  getById(id: number): Observable<ProductModel> {
    return this.getProductByIdUseCase.execute(id);
  }

  update(product: ProductModel): Observable<void> {
    return this.updateProductUseCase.execute(product);
  }
}
