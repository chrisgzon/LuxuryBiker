import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable, map } from 'rxjs';
import { PaginatedResult } from '@domain/common/models/paginated-result.model';
import { ChangeStatusResult } from '@domain/common/models/change-status-result.model';
import {
  CreateSaleModel,
  SaleCreatedModel,
  SaleFormDataModel,
  SaleListItemModel,
} from '@domain/sales/models/sale.model';
import { GetSalesParams, SalesRepository } from '@domain/sales/repositories/sales-repository';
import {
  CreateSaleRequestEntity,
  SaleBriefEntity,
  SaleCreatedEntity,
  SaleFormDataEntity,
} from './sale.entity';

@Injectable({
  providedIn: 'root',
})
export class SalesImplementationRepository extends SalesRepository {
  constructor(private http: HttpClient) {
    super();
  }

  create(params: CreateSaleModel): Observable<SaleCreatedModel> {
    const body: CreateSaleRequestEntity = {
      thirdId: params.thirdId,
      applyIva: params.applyIva,
      details: params.details.map((d) => ({
        productId: d.productId,
        productValue: d.productValue,
        quantity: d.quantity,
      })),
    };

    return this.http
      .post<SaleCreatedEntity>(`${environment.apiUrl}/Sales/Create`, body)
      .pipe(map((entity) => ({ ...entity })));
  }

  changeStatus(id: number): Observable<ChangeStatusResult> {
    return this.http.post<ChangeStatusResult>(`${environment.apiUrl}/Sales/ChangeStatus`, { id });
  }

  getAll(params: GetSalesParams): Observable<PaginatedResult<SaleListItemModel>> {
    let httpParams = new HttpParams()
      .set('pageNumber', String(params.pageNumber ?? 1))
      .set('pageSize', String(params.pageSize ?? 20));

    if (params.dateFrom) {
      httpParams = httpParams.set('dateFrom', params.dateFrom);
    }
    if (params.dateTo) {
      httpParams = httpParams.set('dateTo', params.dateTo);
    }

    return this.http
      .get<PaginatedResult<SaleBriefEntity>>(`${environment.apiUrl}/Sales/GetAll`, {
        params: httpParams,
      })
      .pipe(
        map((response) => ({
          ...response,
          items: response.items.map((item) => ({
            id: item.id,
            code: item.code,
            date: item.date,
            total: item.total,
            status: item.status,
            clientName: item.clientName ?? '—',
            productsQuantity: item.productsQuantity,
          })),
        }))
      );
  }

  getFormData(): Observable<SaleFormDataModel> {
    return this.http
      .get<SaleFormDataEntity>(`${environment.apiUrl}/Sales/GetFormData`)
      .pipe(
        map((entity) => ({
          products: entity.products.map((p) => ({
            id: p.id,
            name: p.name,
            code: p.code ?? '',
            value: p.value ?? 0,
            stock: p.stock ?? 0,
          })),
          clients: entity.clients.map((c) => ({
            id: c.id,
            label: `${`${c.name ?? ''} ${c.surnames ?? ''}`.trim()} (${c.identification})`,
          })),
        }))
      );
  }
}
