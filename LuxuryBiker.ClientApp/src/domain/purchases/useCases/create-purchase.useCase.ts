import { UseCase } from '@base/use-case';
import { Observable } from 'rxjs';
import { CreatePurchaseModel, PurchaseCreatedModel } from '../models/purchase.model';
import { PurchasesRepository } from '../repositories/purchases-repository';

export class CreatePurchaseUseCase
  implements UseCase<CreatePurchaseModel, PurchaseCreatedModel> {

  constructor(private purchasesRepository: PurchasesRepository) { }

  execute(params: CreatePurchaseModel): Observable<PurchaseCreatedModel> {
    return this.purchasesRepository.create(params);
  }
}
