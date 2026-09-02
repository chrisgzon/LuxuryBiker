import { UseCase } from "@base/use-case";
import { Observable } from "rxjs";
import { PaginatedResult } from "@domain/common/models/paginated-result.model";
import { ProductModel } from "../models/product.model";
import { GetProductsParams, ProductsRepository } from "../repositories/products-repository";

export class GetProductsUseCase
  implements UseCase<GetProductsParams, PaginatedResult<ProductModel>> {

  constructor(private productsRepository: ProductsRepository) { }

  execute(params: GetProductsParams): Observable<PaginatedResult<ProductModel>> {
    return this.productsRepository.getAll(params);
  }
}
