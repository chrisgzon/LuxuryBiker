import { Routes } from '@angular/router';
import { isLoggedGuard, isntLoggedGuard } from '@guards/auth/is-logged.guard';

export const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        redirectTo: 'home'
    },
    {
        path: 'login',
        canActivate: [isntLoggedGuard],
        loadComponent: () => import('./dashboard/pages/login/login.component'),
    },
    {
        path: '',
        canActivate: [isLoggedGuard],
        loadComponent: () => import('./dashboard/layouts/master/master.component'),
        children: [
            {
                path: 'home',
                loadComponent: () => import('./dashboard/pages/home/home.component'),
            },
            {
                path: 'thirds',
                children: [
                    {
                        path: 'create',
                        loadComponent: () => import('./dashboard/pages/thirds/create-third/create-third.component'),
                    }
                ],
            },
            {
                path: 'products',
                children: [
                    {
                        path: 'create',
                        loadComponent: () => import('./dashboard/pages/products/create-product/create-product.component'),
                    }
                ]
            }
        ]
    }
];
