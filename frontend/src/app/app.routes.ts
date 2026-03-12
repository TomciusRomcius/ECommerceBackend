import { Routes } from '@angular/router';
import { StorePage } from './views/home/storePage';
import { ProductResolver } from './views/home/productResolver';

export const routes: Routes = [
  {
    path: 'home',
    component: StorePage,
    resolve: { products: ProductResolver }
  },
  {
    path: '*',
    redirectTo: '/store/:id'
  }
];
