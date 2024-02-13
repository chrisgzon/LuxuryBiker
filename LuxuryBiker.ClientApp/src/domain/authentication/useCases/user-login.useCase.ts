import { AuthRepository } from "../repositories/auth-repository";
import { UseCase } from "@base/use-case";
import { LoginCredentialsModel } from "../models/login-credentials.model";
import { Observable } from "rxjs";
import { UserLoggedModel } from "../models/user-logged.model";

export class UserLoginUseCase implements UseCase<LoginCredentialsModel, UserLoggedModel>{

    constructor(private authRepository: AuthRepository) { }

    execute(
       params: LoginCredentialsModel,
    ): Observable<UserLoggedModel> {
        return this.authRepository.login(params);
    }
}