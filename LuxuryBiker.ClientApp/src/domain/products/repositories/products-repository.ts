import { ProductModel } from "@domain/products/models/product.model";
import { Observable } from "rxjs";

export abstract class ProductsRepository {
  abstract create(params: ProductModel): Observable<string>;
}