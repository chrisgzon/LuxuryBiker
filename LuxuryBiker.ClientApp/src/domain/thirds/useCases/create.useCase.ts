import { ThirdsRepository } from "@domain/thirds/repositories/thirds-repository";
import { UseCase } from "@base/use-case";
import { Observable } from "rxjs";
import { ThirdModel } from "@domain/thirds/models/third.model";

export class ThirdCreateUseCase implements UseCase<ThirdModel, number>{ // TODO: spicify the return type

    constructor(private thirdsRepository: ThirdsRepository) { }

    execute(
       params: ThirdModel,
    ): Observable<number> {
        return this.thirdsRepository.create(params);
    }
}