import { Injectable } from '@angular/core';
import { ProductModel } from '@domain/products/models/product.model';
import { ProductCreateUseCase } from '@domain/products/useCases/create.useCase';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {

  constructor(private productCreateUseCase: ProductCreateUseCase) { }

  create(product: ProductModel): Observable<string> {
      return this.productCreateUseCase.execute(product);
    }
}
