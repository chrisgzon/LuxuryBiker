import { UseCase } from '@base/use-case';
import { Observable } from 'rxjs';
import { ThirdModel } from '../models/third.model';
import { ThirdsRepository } from '../repositories/thirds-repository';

export class UpdateThirdUseCase implements UseCase<ThirdModel, void> {
  constructor(private thirdsRepository: ThirdsRepository) {}

  execute(params: ThirdModel): Observable<void> {
    return this.thirdsRepository.update(params);
  }
}
