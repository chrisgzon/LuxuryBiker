import { AuthRepository } from "../repositories/auth-repository";
import { UseCase } from "@base/use-case";
import { LoginCredentialsModel } from "../models/login-credentials.model";
import { Observable } from "rxjs";

export class UserLoginUseCase implements UseCase<LoginCredentialsModel, string>{

    constructor(private authRepository: AuthRepository) { }

    execute(
       params: LoginCredentialsModel,
    ): Observable<string> {
        return this.authRepository.login(params);
    }
}