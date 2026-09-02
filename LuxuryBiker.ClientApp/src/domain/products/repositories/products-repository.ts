import { ProductModel } from "@domain/products/models/product.model";
import { PaginatedResult, PaginationParams } from "@domain/common/models/paginated-result.model";
import { Observable } from "rxjs";

export interface GetProductsParams extends PaginationParams {
  onlyActive?: boolean;
}

export abstract class ProductsRepository {
  abstract create(params: ProductModel): Observable<string>;
  abstract getAll(params: GetProductsParams): Observable<PaginatedResult<ProductModel>>;
  abstract getById(id: number): Observable<ProductModel>;
  abstract update(params: ProductModel): Observable<void>;
}
