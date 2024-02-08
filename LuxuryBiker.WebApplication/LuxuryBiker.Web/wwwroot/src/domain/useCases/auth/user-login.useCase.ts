import { UserRepository } from "@abstract/repositories/user-repository";
import { UseCase } from "@base/use-case";
import { LoginCredentialsModel } from "@models/auth/login-credentials.model";
import { UserLoggedModel } from "@models/auth/user-logged.model";
import { Observable } from "rxjs";

export class UserLoginUseCase implements UseCase<LoginCredentialsModel, UserLoggedModel>{

    constructor(private userRepository: UserRepository) { }

    execute(
       params: { username: string, password: string },
    ): Observable<UserLoggedModel> {
        return this.userRepository.login(params);
    }
}