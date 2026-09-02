import { UseCase } from '@base/use-case';
import { Observable } from 'rxjs';
import { ThirdModel } from '../models/third.model';
import { ThirdsRepository } from '../repositories/thirds-repository';

export class GetThirdByIdUseCase implements UseCase<number, ThirdModel> {
  constructor(private thirdsRepository: ThirdsRepository) {}

  execute(id: number): Observable<ThirdModel> {
    return this.thirdsRepository.getById(id);
  }
}
