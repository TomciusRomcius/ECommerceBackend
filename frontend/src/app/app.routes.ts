import { Routes } from '@angular/router';
import { StorePage } from './views/home/storePage';
import { productResolver } from './views/home/product.resolver';
import { loginRedirectGuard } from './guards/login-redirect.guard';
import { AuthCallback } from './views/auth/auth-callback';
import { ProductPage } from './views/product/product-page';
import { productDetailResolver } from './views/product/product-detail.resolver';
import { CartPage } from './views/cart/cartPage';
import { CheckoutPage } from './views/checkout/checkout-page';
import { StoresPage } from './views/stores/stores-page';
import { storeLocationsResolver } from './views/stores/store-locations.resolver';
import { CreateManufacturerPage } from './views/create-manufacturer/create-manufacturer-page';
import { CreateCategoryPage } from './views/create-category/create-category-page';

export const routes: Routes = [
  {
    path: 'home',
    component: StorePage,
    resolve: { products: productResolver },
    runGuardsAndResolvers: 'paramsOrQueryParamsChange',
  },
  {
    path: 'stores',
    component: StoresPage,
    resolve: { storeLocations: storeLocationsResolver },
  },
  {
    path: 'product/:productId',
    component: ProductPage,
    resolve: { product: productDetailResolver },
  },
  {
    path: 'cart',
    component: CartPage,
  },
  {
    path: 'checkout',
    component: CheckoutPage,
  },
  {
    path: 'manufacturers/create',
    component: CreateManufacturerPage,
  },
  {
    path: 'categories/create',
    component: CreateCategoryPage,
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
