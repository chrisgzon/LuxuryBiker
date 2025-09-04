import { Injectable } from '@angular/core';
import { ThirdModel } from '@domain/thirds/models/third.model';
import { ThirdCreateUseCase } from '@domain/thirds/useCases/create.useCase';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ThirdsService {
  constructor(
    private thirdCreateUseCase: ThirdCreateUseCase
  ) {
  }

  create(third: ThirdModel): Observable<number> {
    return this.thirdCreateUseCase.execute(third);
  }
}
