import { UseCase } from '@base/use-case';
import { Observable } from 'rxjs';
import { SaleFormDataModel } from '../models/sale.model';
import { SalesRepository } from '../repositories/sales-repository';

export class GetSaleFormDataUseCase implements UseCase<void, SaleFormDataModel> {
  constructor(private salesRepository: SalesRepository) {}

  execute(): Observable<SaleFormDataModel> {
    return this.salesRepository.getFormData();
  }
}
