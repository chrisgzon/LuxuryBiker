import { UseCase } from '@base/use-case';
import { Observable } from 'rxjs';
import { PaginatedResult } from '@domain/common/models/paginated-result.model';
import { PurchaseListItemModel } from '../models/purchase.model';
import { GetPurchasesParams, PurchasesRepository } from '../repositories/purchases-repository';

export class GetPurchasesUseCase
  implements UseCase<GetPurchasesParams, PaginatedResult<PurchaseListItemModel>> {

  constructor(private purchasesRepository: PurchasesRepository) { }

  execute(params: GetPurchasesParams): Observable<PaginatedResult<PurchaseListItemModel>> {
    return this.purchasesRepository.getAll(params);
  }
}
