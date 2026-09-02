import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable, map } from 'rxjs';
import { PaginatedResult } from '@domain/common/models/paginated-result.model';
import { ChangeStatusResult } from '@domain/common/models/change-status-result.model';
import {
  CreatePurchaseModel,
  PurchaseCreatedModel,
  PurchaseFormDataModel,
  PurchaseListItemModel,
} from '@domain/purchases/models/purchase.model';
import {
  GetPurchasesParams,
  PurchasesRepository,
} from '@domain/purchases/repositories/purchases-repository';
import {
  CreatePurchaseRequestEntity,
  PurchaseBriefEntity,
  PurchaseCreatedEntity,
  PurchaseFormDataEntity,
} from './purchase.entity';

@Injectable({
  providedIn: 'root',
})
export class PurchasesImplementationRepository extends PurchasesRepository {
  constructor(private http: HttpClient) {
    super();
  }

  create(params: CreatePurchaseModel): Observable<PurchaseCreatedModel> {
    const body: CreatePurchaseRequestEntity = {
      thirdId: params.thirdId,
      datePurchase: params.datePurchase,
      applyIva: params.applyIva,
      details: params.details.map((d) => ({
        productId: d.productId,
        productValue: d.productValue,
        quantity: d.quantity,
      })),
    };

    return this.http
      .post<PurchaseCreatedEntity>(`${environment.apiUrl}/Purchases/Create`, body)
      .pipe(map((entity) => ({ ...entity })));
  }

  changeStatus(id: number): Observable<ChangeStatusResult> {
    return this.http.post<ChangeStatusResult>(
      `${environment.apiUrl}/Purchases/ChangeStatus`,
      { id }
    );
  }

  getAll(params: GetPurchasesParams): Observable<PaginatedResult<PurchaseListItemModel>> {
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
      .get<PaginatedResult<PurchaseBriefEntity>>(`${environment.apiUrl}/Purchases/GetAll`, {
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
            supplierName: item.supplierName ?? '—',
            productsQuantity: item.productsQuantity,
          })),
        }))
      );
  }

  getFormData(): Observable<PurchaseFormDataModel> {
    return this.http
      .get<PurchaseFormDataEntity>(`${environment.apiUrl}/Purchases/GetFormData`)
      .pipe(
        map((entity) => ({
          products: entity.products.map((p) => ({
            id: p.id,
            name: p.name,
            code: p.code ?? '',
            value: p.value ?? 0,
            stock: p.stock ?? 0,
          })),
          suppliers: entity.suppliers.map((s) => ({
            id: s.id,
            label: `${`${s.name ?? ''} ${s.surnames ?? ''}`.trim()} (${s.identification})`,
          })),
        }))
      );
  }
}
