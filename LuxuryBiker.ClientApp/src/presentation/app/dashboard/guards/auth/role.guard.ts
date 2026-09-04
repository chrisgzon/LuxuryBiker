import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { filter, map, take } from 'rxjs';
import { AuthService } from '@services/auth/auth.service';

/**
 * Permite la ruta solo si el usuario tiene alguno de los roles indicados.
 * Espera a que el perfil se haya resuelto para no rechazar por una carrera de
 * inicialización; si no cumple, redirige a `/home`.
 */
export const roleGuard = (...allowedRoles: string[]): CanActivateFn => {
  return () => {
    const authService = inject(AuthService);
    const router = inject(Router);

    return authService.profileResolved$.pipe(
      filter(Boolean),
      take(1),
      map(() =>
        authService.hasAnyRole(...allowedRoles) ? true : router.createUrlTree(['/home'])
      )
    );
  };
};
