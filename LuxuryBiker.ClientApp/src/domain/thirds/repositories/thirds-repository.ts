import { ThirdModel } from "@domain/thirds/models/third.model";
import { PaginatedResult, PaginationParams } from "@domain/common/models/paginated-result.model";
import { Observable } from "rxjs";

export interface GetThirdsParams extends PaginationParams {
  typeId?: number;
}

export abstract class ThirdsRepository {
  abstract create(params: ThirdModel): Observable<number>;
  abstract getAll(params: GetThirdsParams): Observable<PaginatedResult<ThirdModel>>;
  abstract getById(id: number): Observable<ThirdModel>;
  abstract update(params: ThirdModel): Observable<void>;
}
