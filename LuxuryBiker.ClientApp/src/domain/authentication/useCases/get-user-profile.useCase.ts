import { AuthRepository } from "../repositories/auth-repository";
import { UseCase } from "@base/use-case";
import { Observable } from "rxjs";
import { UserLoggedModel } from "../models/user-logged.model";

export class GetUserProfileUseCase implements UseCase<void, UserLoggedModel>{

    constructor(private authRepository: AuthRepository) { }

    execute():
     Observable<UserLoggedModel> {
        return this.authRepository.getProfileCurrentUser();
    }
}