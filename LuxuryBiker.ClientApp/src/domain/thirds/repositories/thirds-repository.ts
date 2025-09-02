import { ThirdModel } from "@domain/thirds/models/third.model";
import { Observable } from "rxjs";

export abstract class ThirdsRepository {
  abstract create(params: ThirdModel): Observable<number>;
}