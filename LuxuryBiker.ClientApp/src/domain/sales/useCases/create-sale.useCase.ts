import { UseCase } from '@base/use-case';
import { Observable } from 'rxjs';
import { CreateSaleModel, SaleCreatedModel } from '../models/sale.model';
import { SalesRepository } from '../repositories/sales-repository';

export class CreateSaleUseCase implements UseCase<CreateSaleModel, SaleCreatedModel> {
  constructor(private salesRepository: SalesRepository) {}

  execute(params: CreateSaleModel): Observable<SaleCreatedModel> {
    return this.salesRepository.create(params);
  }
}
