import { UseCase } from '@base/use-case';
import { Observable } from 'rxjs';
import { PurchaseFormDataModel } from '../models/purchase.model';
import { PurchasesRepository } from '../repositories/purchases-repository';

export class GetPurchaseFormDataUseCase
  implements UseCase<void, PurchaseFormDataModel> {

  constructor(private purchasesRepository: PurchasesRepository) { }

  execute(): Observable<PurchaseFormDataModel> {
    return this.purchasesRepository.getFormData();
  }
}
