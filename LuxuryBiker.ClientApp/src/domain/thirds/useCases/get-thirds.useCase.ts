import { UseCase } from "@base/use-case";
import { Observable } from "rxjs";
import { PaginatedResult } from "@domain/common/models/paginated-result.model";
import { ThirdModel } from "../models/third.model";
import { GetThirdsParams, ThirdsRepository } from "../repositories/thirds-repository";

export class GetThirdsUseCase
  implements UseCase<GetThirdsParams, PaginatedResult<ThirdModel>> {

  constructor(private thirdsRepository: ThirdsRepository) { }

  execute(params: GetThirdsParams): Observable<PaginatedResult<ThirdModel>> {
    return this.thirdsRepository.getAll(params);
  }
}
