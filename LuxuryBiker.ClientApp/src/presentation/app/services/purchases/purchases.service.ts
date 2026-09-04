import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginatedResult } from '@domain/common/models/paginated-result.model';
import { ChangeStatusResult } from '@domain/common/models/change-status-result.model';
import {
  CreatePurchaseModel,
  PurchaseCreatedModel,
  PurchaseFormDataModel,
  PurchaseListItemModel,
} from '@domain/purchases/models/purchase.model';
import { GetPurchasesParams } from '@domain/purchases/repositories/purchases-repository';
import { CreatePurchaseUseCase } from '@domain/purchases/useCases/create-purchase.useCase';
import { GetPurchasesUseCase } from '@domain/purchases/useCases/get-purchases.useCase';
import { GetPurchaseFormDataUseCase } from '@domain/purchases/useCases/get-purchase-form-data.useCase';
import { ChangePurchaseStatusUseCase } from '@domain/purchases/useCases/change-purchase-status.useCase';

@Injectable({
  providedIn: 'root',
})
export class PurchasesService {
  constructor(
    private createPurchaseUseCase: CreatePurchaseUseCase,
    private getPurchasesUseCase: GetPurchasesUseCase,
    private getPurchaseFormDataUseCase: GetPurchaseFormDataUseCase,
    private changePurchaseStatusUseCase: ChangePurchaseStatusUseCase
  ) {}

  create(purchase: CreatePurchaseModel): Observable<PurchaseCreatedModel> {
    return this.createPurchaseUseCase.execute(purchase);
  }

  getAll(params: GetPurchasesParams = {}): Observable<PaginatedResult<PurchaseListItemModel>> {
    return this.getPurchasesUseCase.execute(params);
  }

  getFormData(): Observable<PurchaseFormDataModel> {
    return this.getPurchaseFormDataUseCase.execute();
  }

  changeStatus(purchaseId: number): Observable<ChangeStatusResult> {
    return this.changePurchaseStatusUseCase.execute(purchaseId);
  }
}
