import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        redirectTo: 'home'
    },
    {
        path: '',
        loadComponent: () => import('./dashboard/layouts/master/master.component'),
        children: [
            {
                path: 'home',
                loadComponent: () => import('./dashboard/pages/home/home.component')
            }
        ]
    },
];
