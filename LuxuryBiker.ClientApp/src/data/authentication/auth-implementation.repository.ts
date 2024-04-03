import { AuthRepository } from "@domain/authentication/repositories/auth-repository";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@enviroments/enviroment";
import { LoginCredentialsModel } from "@domain/authentication/models/login-credentials.model";
import { Observable, map } from "rxjs";
import { UserLoggedModel } from "@domain/authentication/models/user-logged.model";
import { UserAuthImplementationRepositoryMapper } from "./user-auth-implementation-repository.mapper";
import { UserLoggedEntity } from "./user-logged.entity";


@Injectable({
    providedIn: 'root',
})
export class AuthImplementationRepository extends AuthRepository {
    userMapper = new UserAuthImplementationRepositoryMapper();

    constructor(private http: HttpClient) {
        super();
    }

    login(params: LoginCredentialsModel): Observable<string> {
        return this.http
            .post<string>(`${environment.apiUrl}/Authentication/Login`, params);
    }

    getProfileCurrentUser(): Observable<UserLoggedModel> {
        return this.http
            .get<UserLoggedEntity>(`${environment.apiUrl}/Authentication/GetProfileCurrentUser`)
            .pipe(map(this.userMapper.mapFrom));
    }
}