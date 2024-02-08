import { LoginCredentialsModel } from "@models/auth/login-credentials.model";
import { UserLoggedModel } from "@models/auth/user-logged.model";
import { UserModel } from "@models/user.model";
import { Observable } from "rxjs";

export abstract class UserRepository {
  abstract login(params: LoginCredentialsModel): Observable<UserLoggedModel>;
  abstract register(params: UserModel): Observable<UserLoggedModel>;
  abstract getUserProfile(): Observable<UserLoggedModel>;
}