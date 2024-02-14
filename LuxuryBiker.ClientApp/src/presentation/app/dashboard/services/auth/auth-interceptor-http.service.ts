import { inject } from '@angular/core';
import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, first, switchMap, throwError } from 'rxjs';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';

export const AuthInterceptorHttpService: HttpInterceptorFn = (req, next) => {

  const router: Router = inject(Router);
  const jwt: string|null = localStorage.getItem("userData");

  if (jwt) {
    req = req.clone({
      setHeaders: {
        authorization: `Bearer ${ jwt }`
      }
    });
  }

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {

      if (err.status === 401) {
        router.navigateByUrl('/login');
      }

      return throwError(() => err );
    })
  )
}