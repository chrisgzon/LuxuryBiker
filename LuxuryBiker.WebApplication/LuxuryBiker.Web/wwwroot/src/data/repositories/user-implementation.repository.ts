import { UserRepository } from "@abstract/repositories/user-repository";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { UserEntity } from "@entities/user.entity";
import { UserAuthImplementationRepositoryMapper } from "@mappers/user-auth-repository-implementation-repository.mapper";
import { LoginCredentialsModel } from "@models/auth/login-credentials.model";
import { UserLoggedModel } from "@models/auth/user-logged.model";
import { UserModel } from "@models/user.model";
import { Observable, map } from "rxjs";


@Injectable({
    providedIn: 'root',
})
export class UserImplementationRepository extends UserRepository {
    userMapper = new UserAuthImplementationRepositoryMapper();

    constructor(private http: HttpClient) {
        super();
    }

    login(params: LoginCredentialsModel): Observable<UserLoggedModel> {
        return this.http
            .post<UserEntity>('/login', {params})
            .pipe(map(this.userMapper.mapFrom));
    }
    register(params: UserModel): Observable<UserLoggedModel> {
       return this.http
            .post<UserEntity>('/register', {params})
            .pipe(map(this.userMapper.mapFrom));
    }
    getUserProfile(): Observable<UserLoggedModel>{
        return this.http.get<UserEntity>('/user').pipe(
            map(this.userMapper.mapFrom));
    }
}
