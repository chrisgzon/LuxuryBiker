import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable, map } from 'rxjs';
import { GetProductsParams, ProductsRepository } from '@domain/products/repositories/products-repository';
import { ProductModel } from '@domain/products/models/product.model';
import { PaginatedResult } from '@domain/common/models/paginated-result.model';
import { ProductEntity } from './product.entity';
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
    const productEntity = this.ProductsMapper.mapTo(params);
    return this.http.post<string>(
      `${environment.apiUrl}/Products/Create`,
      productEntity
    );
  }

  getById(id: number): Observable<ProductModel> {
    return this.http
      .get<ProductEntity>(`${environment.apiUrl}/Products/GetById`, {
        params: new HttpParams().set('id', String(id)),
      })
      .pipe(map((entity) => this.ProductsMapper.mapFrom(entity)));
  }

  update(params: ProductModel): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/Products/Update`, {
      id: params.id,
      name: params.name,
      reference: params.reference,
      status: params.status,
      description: params.description,
    });
  }

  getAll(params: GetProductsParams): Observable<PaginatedResult<ProductModel>> {
    const httpParams = new HttpParams()
      .set('pageNumber', String(params.pageNumber ?? 1))
      .set('pageSize', String(params.pageSize ?? 20))
      .set('onlyActive', String(params.onlyActive ?? true));

    return this.http
      .get<PaginatedResult<ProductEntity>>(`${environment.apiUrl}/Products/GetAll`, {
        params: httpParams,
      })
      .pipe(
        map((response) => ({
          ...response,
          items: response.items.map((item) => this.ProductsMapper.mapFrom(item)),
        }))
      );
  }
}
