import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginatedResult } from '@domain/common/models/paginated-result.model';
import { ChangeStatusResult } from '@domain/common/models/change-status-result.model';
import {
  CreateSaleModel,
  SaleCreatedModel,
  SaleFormDataModel,
  SaleListItemModel,
} from '@domain/sales/models/sale.model';
import { GetSalesParams } from '@domain/sales/repositories/sales-repository';
import { CreateSaleUseCase } from '@domain/sales/useCases/create-sale.useCase';
import { GetSalesUseCase } from '@domain/sales/useCases/get-sales.useCase';
import { GetSaleFormDataUseCase } from '@domain/sales/useCases/get-sale-form-data.useCase';
import { ChangeSaleStatusUseCase } from '@domain/sales/useCases/change-sale-status.useCase';

@Injectable({
  providedIn: 'root',
})
export class SalesService {
  constructor(
    private createSaleUseCase: CreateSaleUseCase,
    private getSalesUseCase: GetSalesUseCase,
    private getSaleFormDataUseCase: GetSaleFormDataUseCase,
    private changeSaleStatusUseCase: ChangeSaleStatusUseCase
  ) {}

  create(sale: CreateSaleModel): Observable<SaleCreatedModel> {
    return this.createSaleUseCase.execute(sale);
  }

  getAll(params: GetSalesParams = {}): Observable<PaginatedResult<SaleListItemModel>> {
    return this.getSalesUseCase.execute(params);
  }

  getFormData(): Observable<SaleFormDataModel> {
    return this.getSaleFormDataUseCase.execute();
  }

  changeStatus(saleId: number): Observable<ChangeStatusResult> {
    return this.changeSaleStatusUseCase.execute(saleId);
  }
}
