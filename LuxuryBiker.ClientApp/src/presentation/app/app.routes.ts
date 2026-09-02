import { Routes } from '@angular/router';
import { isLoggedGuard, isntLoggedGuard } from '@guards/auth/is-logged.guard';
import { roleGuard } from '@guards/auth/role.guard';
import { ROLES } from '@domain/authentication/models/user-logged.model';

const manageGuard = roleGuard(ROLES.administrator, ROLES.seller);

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'home',
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
        canActivate: [manageGuard],
        children: [
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'list',
          },
          {
            path: 'list',
            loadComponent: () =>
              import('./dashboard/pages/thirds/list-thirds/list-thirds.component'),
          },
          {
            path: 'create',
            loadComponent: () =>
              import(
                './dashboard/pages/thirds/create-third/create-third.component'
              ),
          },
          {
            path: 'edit/:id',
            loadComponent: () =>
              import('./dashboard/pages/thirds/edit-third/edit-third.component'),
          },
        ],
      },
      {
        path: 'products',
        canActivate: [manageGuard],
        children: [
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'list',
          },
          {
            path: 'list',
            loadComponent: () =>
              import(
                './dashboard/pages/products/list-products/list-products.component'
              ),
          },
          {
            path: 'create',
            loadComponent: () =>
              import(
                './dashboard/pages/products/create-product/create-product.component'
              ),
          },
          {
            path: 'edit/:id',
            loadComponent: () =>
              import(
                './dashboard/pages/products/edit-product/edit-product.component'
              ),
          },
        ],
      },
      {
        path: 'purchases',
        canActivate: [manageGuard],
        children: [
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'list',
          },
          {
            path: 'list',
            loadComponent: () =>
              import(
                './dashboard/pages/purchases/list-purchases/list-purchases.component'
              ),
          },
          {
            path: 'register',
            loadComponent: () =>
              import(
                './dashboard/pages/purchases/register-purchase/register-purchase.component'
              ),
          },
        ],
      },
      {
        path: 'sales',
        canActivate: [manageGuard],
        children: [
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'list',
          },
          {
            path: 'list',
            loadComponent: () =>
              import('./dashboard/pages/sales/list-sales/list-sales.component'),
          },
          {
            path: 'register',
            loadComponent: () =>
              import('./dashboard/pages/sales/register-sale/register-sale.component'),
          },
        ],
      },
    ],
  },
];
