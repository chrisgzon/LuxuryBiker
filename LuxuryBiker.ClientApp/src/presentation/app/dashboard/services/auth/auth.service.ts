import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { LoginCredentialsModel } from '@domain/authentication/models/login-credentials.model';
import { UserLoggedModel } from '@domain/authentication/models/user-logged.model';
import { BehaviorSubject, Observable, catchError, ignoreElements, map, tap } from 'rxjs';
import { UserLoginUseCase } from '@domain/authentication/useCases/user-login.useCase';
import { GetUserProfileUseCase } from '@domain/authentication/useCases/get-user-profile.useCase';
import { HttpErrorResponse } from '@angular/common/http';

const USER_LOCAL_STORAGE_KEY = 'userData';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private user = new BehaviorSubject<UserLoggedModel | null>(null);
  user$ = this.user.asObservable();
  isLoggedIn$: Observable<boolean> = this.user$.pipe(map(Boolean));

  constructor(
    private userLoginUseCase: UserLoginUseCase,
    private getUserProfileUseCase: GetUserProfileUseCase,
    private router: Router)
  {
    this.loadUserFromLocalStorage();
  }

  login(credentials: LoginCredentialsModel): Observable<never> {
    return this.userLoginUseCase.execute(credentials)
    .pipe(
      tap((user: UserLoggedModel) => this.saveTokenToLocalStore(user.token)),
      tap((user: UserLoggedModel) => this.pushNewUser(user)),
      tap(() => this.redirectToDashboard()),
      ignoreElements()
    );
  }

  logout(): void {
    this.removeUserFromLocalStorage();
    this.router.navigateByUrl('/login');
  }
  
  private getProfileCurrentUser(): Observable<never> {
    return this.getUserProfileUseCase.execute()
    .pipe(
      tap((userProfile: UserLoggedModel) => this.pushNewUser(userProfile)),
      ignoreElements(),
      catchError((error: HttpErrorResponse) => {
        this.removeUserFromLocalStorage();
        this.router.navigateByUrl("/login");
        throw error;
      })
    );
  }

  private getToken(): string|null {
    return localStorage.getItem(USER_LOCAL_STORAGE_KEY);
  }
    
  private loadUserFromLocalStorage(): void {
    const jwtFromLocal = this.getToken();
    
    jwtFromLocal 
    && this.getProfileCurrentUser().subscribe()
    && this.pushNewUser({ token: jwtFromLocal } as UserLoggedModel);
  }
  
  private redirectToDashboard(): void {
    this.router.navigateByUrl('/home');
  }

  private pushNewUser(userProfile: UserLoggedModel|null) {
    this.user.next(userProfile);
  }

  private saveTokenToLocalStore(userToken: string): void {
    localStorage.setItem(USER_LOCAL_STORAGE_KEY, userToken);
  }

  private removeUserFromLocalStorage(): void {
    localStorage.removeItem(USER_LOCAL_STORAGE_KEY);
    this.pushNewUser(null);
  }
}
