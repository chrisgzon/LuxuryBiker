import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@enviroments/enviroment';
import { Observable } from 'rxjs';
import { ProductsRepository } from '@domain/products/repositories/products-repository';
import { ProductModel } from '@domain/products/models/product.model';
import { ProductImplementationRepositoryMapper } from './product-implementation-repository.mapper';

@Injectable({
  providedIn: 'root',
})
export class ProductsImplementationRepository extends ProductsRepository {
  ProductsMapper = new ProductImplementationRepositoryMapper();

  constructor(private http: HttpClient) {
    super();
  }

  create(params: ProductModel): Observable<string> {
    const productEntity = this.ProductsMapper
      .mapTo(params);
    return this.http.post<string>(
      `${environment.apiUrl}/Products/Create`,
      productEntity
    );
  }
}
