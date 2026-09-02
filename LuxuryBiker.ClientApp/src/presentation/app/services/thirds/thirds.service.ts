import { Injectable } from '@angular/core';
import { ThirdModel } from '@domain/thirds/models/third.model';
import { ThirdCreateUseCase } from '@domain/thirds/useCases/create.useCase';
import { GetThirdsUseCase } from '@domain/thirds/useCases/get-thirds.useCase';
import { GetThirdByIdUseCase } from '@domain/thirds/useCases/get-third-by-id.useCase';
import { UpdateThirdUseCase } from '@domain/thirds/useCases/update-third.useCase';
import { GetThirdsParams } from '@domain/thirds/repositories/thirds-repository';
import { PaginatedResult } from '@domain/common/models/paginated-result.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ThirdsService {
  constructor(
    private thirdCreateUseCase: ThirdCreateUseCase,
    private getThirdsUseCase: GetThirdsUseCase,
    private getThirdByIdUseCase: GetThirdByIdUseCase,
    private updateThirdUseCase: UpdateThirdUseCase
  ) {
  }

  create(third: ThirdModel): Observable<number> {
    return this.thirdCreateUseCase.execute(third);
  }

  getAll(params: GetThirdsParams = {}): Observable<PaginatedResult<ThirdModel>> {
    return this.getThirdsUseCase.execute(params);
  }

  getById(id: number): Observable<ThirdModel> {
    return this.getThirdByIdUseCase.execute(id);
  }

  update(third: ThirdModel): Observable<void> {
    return this.updateThirdUseCase.execute(third);
  }
}
