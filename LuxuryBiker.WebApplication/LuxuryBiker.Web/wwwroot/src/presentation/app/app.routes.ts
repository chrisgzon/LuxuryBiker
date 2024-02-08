import { Routes } from '@angular/router';
import { isLoggedGuard } from '@guards/auth/is-logged.guard';

export const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        redirectTo: 'home'
    },
    {
        path: 'login',
        loadComponent: () => import('./dashboard/pages/login/login.component'),
    },
    {
        path: '',
        canActivate: [isLoggedGuard],
        loadComponent: () => import('./dashboard/layouts/master/master.component'),
        children: [
            {
                path: 'home',
                loadComponent: () => import('./dashboard/pages/home/home.component')
            }
        ]
    },
];
