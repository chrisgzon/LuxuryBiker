import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '@services/auth/auth.service';
import { map } from 'rxjs';

export const isLoggedGuard: CanActivateFn = (route, state) => {

  const router: Router = inject(Router);

  return inject(AuthService).isLoggedIn$.pipe(
    map((isLoggedIn) => isLoggedIn || router.createUrlTree(['/login']))
  );
};

export const isntLoggedGuard: CanActivateFn = (route, state) => {

  const router: Router = inject(Router);
  return inject(AuthService).isLoggedIn$.pipe(
      map((isLoggedIn) => {
        if (isLoggedIn) {
          return router.createUrlTree(['/home']);
        }
        return true;
      }
    )
  );
};
