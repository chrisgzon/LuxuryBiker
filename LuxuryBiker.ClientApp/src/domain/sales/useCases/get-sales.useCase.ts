import { UseCase } from '@base/use-case';
import { Observable } from 'rxjs';
import { PaginatedResult } from '@domain/common/models/paginated-result.model';
import { SaleListItemModel } from '../models/sale.model';
import { GetSalesParams, SalesRepository } from '../repositories/sales-repository';

export class GetSalesUseCase
  implements UseCase<GetSalesParams, PaginatedResult<SaleListItemModel>> {

  constructor(private salesRepository: SalesRepository) {}

  execute(params: GetSalesParams): Observable<PaginatedResult<SaleListItemModel>> {
    return this.salesRepository.getAll(params);
  }
}
