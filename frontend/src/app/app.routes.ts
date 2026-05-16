import { Routes } from '@angular/router';
import { StorePage } from './views/home/storePage';
import { productResolver } from './views/home/productResolver';

export const routes: Routes = [
  {
    path: 'home',
    component: StorePage,
    resolve: { products: productResolver }
  },
  {
    path: '**',
    redirectTo: '/home'
  }
];
