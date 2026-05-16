import { Routes } from '@angular/router';
import { StorePage } from './views/home/storePage';
import { productResolver } from './views/home/productResolver';
import { loginRedirectGuard } from './guards/login-redirect.guard';
import { AuthCallback } from './views/auth/auth-callback';

export const routes: Routes = [
  {
    path: 'home',
    component: StorePage,
    resolve: { products: productResolver }
  },
  {
    path: 'auth/callback',
    component: AuthCallback,
  },
  {
    path: 'login',
    canActivate: [loginRedirectGuard],
    component: StorePage,
  },
  {
    path: '**',
    redirectTo: '/home'
  }
];
