import { UseCase } from "@base/use-case";
import { Observable } from "rxjs";
import { ProductModel } from "../models/product.model";
import { ProductsRepository } from "../repositories/products-repository";

export class ProductCreateUseCase implements UseCase<ProductModel, string>{

    constructor(private ProductsRepository: ProductsRepository) { }

    execute(
       params: ProductModel,
    ): Observable<string> {
        return this.ProductsRepository.create(params);
    }
}