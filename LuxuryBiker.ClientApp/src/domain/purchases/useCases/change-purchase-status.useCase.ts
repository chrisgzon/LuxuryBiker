import { UseCase } from '@base/use-case';
import { Observable } from 'rxjs';
import { ChangeStatusResult } from '@domain/common/models/change-status-result.model';
import { PurchasesRepository } from '../repositories/purchases-repository';

export class ChangePurchaseStatusUseCase implements UseCase<number, ChangeStatusResult> {
  constructor(private purchasesRepository: PurchasesRepository) {}

  execute(purchaseId: number): Observable<ChangeStatusResult> {
    return this.purchasesRepository.changeStatus(purchaseId);
  }
}
