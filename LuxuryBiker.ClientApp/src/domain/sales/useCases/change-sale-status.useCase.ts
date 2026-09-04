import { UseCase } from '@base/use-case';
import { Observable } from 'rxjs';
import { ChangeStatusResult } from '@domain/common/models/change-status-result.model';
import { SalesRepository } from '../repositories/sales-repository';

export class ChangeSaleStatusUseCase implements UseCase<number, ChangeStatusResult> {
  constructor(private salesRepository: SalesRepository) {}

  execute(saleId: number): Observable<ChangeStatusResult> {
    return this.salesRepository.changeStatus(saleId);
  }
}
