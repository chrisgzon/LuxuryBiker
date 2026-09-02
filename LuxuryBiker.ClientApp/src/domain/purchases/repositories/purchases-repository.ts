import { Observable } from 'rxjs';
import { PaginatedResult, PaginationParams } from '@domain/common/models/paginated-result.model';
import { ChangeStatusResult } from '@domain/common/models/change-status-result.model';
import {
  CreatePurchaseModel,
  PurchaseCreatedModel,
  PurchaseFormDataModel,
  PurchaseListItemModel,
} from '@domain/purchases/models/purchase.model';

export interface GetPurchasesParams extends PaginationParams {
  dateFrom?: string;
  dateTo?: string;
}

export abstract class PurchasesRepository {
  abstract create(params: CreatePurchaseModel): Observable<PurchaseCreatedModel>;
  abstract getAll(params: GetPurchasesParams): Observable<PaginatedResult<PurchaseListItemModel>>;
  abstract getFormData(): Observable<PurchaseFormDataModel>;
  abstract changeStatus(id: number): Observable<ChangeStatusResult>;
}
