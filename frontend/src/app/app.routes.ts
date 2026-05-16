import { Routes } from '@angular/router';
import { StorePage } from './views/home/storePage';
import { productResolver } from './views/home/productResolver';
import { loginRedirectGuard } from './guards/login-redirect.guard';
import { AuthCallback } from './views/auth/auth-callback';
import { ProductPage } from './views/product/productPage';
import { productDetailResolver } from './views/product/productDetailResolver';

export const routes: Routes = [
  {
    path: 'home',
    component: StorePage,
    resolve: { products: productResolver }
  },
  {
    path: 'product/:productId',
    component: ProductPage,
    resolve: { product: productDetailResolver },
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
