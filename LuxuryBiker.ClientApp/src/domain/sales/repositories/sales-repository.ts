import { Observable } from 'rxjs';
import { PaginatedResult, PaginationParams } from '@domain/common/models/paginated-result.model';
import { ChangeStatusResult } from '@domain/common/models/change-status-result.model';
import {
  CreateSaleModel,
  SaleCreatedModel,
  SaleFormDataModel,
  SaleListItemModel,
} from '@domain/sales/models/sale.model';

export interface GetSalesParams extends PaginationParams {
  dateFrom?: string;
  dateTo?: string;
}

export abstract class SalesRepository {
  abstract create(params: CreateSaleModel): Observable<SaleCreatedModel>;
  abstract getAll(params: GetSalesParams): Observable<PaginatedResult<SaleListItemModel>>;
  abstract getFormData(): Observable<SaleFormDataModel>;
  abstract changeStatus(id: number): Observable<ChangeStatusResult>;
}
