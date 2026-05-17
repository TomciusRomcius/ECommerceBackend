import { CurrencyPipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { PagePadder } from '../../components/page-padder/page-padder';
import { ACCESS_TOKEN_KEY } from '../../constants/auth';
import ProductModel from '../../models/product-model';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-product-page',
  imports: [CurrencyPipe, MatButtonModule, PagePadder, RouterLink],
  templateUrl: './product-page.html',
})
export class ProductPage {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly cartService = inject(CartService);

  product = signal<ProductModel | null>(null);
  addingToCart = signal(false);
  cartMessage = signal<string | null>(null);

  constructor() {
    this.route.data.subscribe(({ product }) => {
      this.product.set(product);
    });
  }

  addToCart(): void {
    const product = this.product();
    if (!product) {
      return;
    }

    if (!sessionStorage.getItem(ACCESS_TOKEN_KEY)) {
      void this.router.navigate(['/login']);
      return;
    }

    this.addingToCart.set(true);
    this.cartMessage.set(null);

    this.cartService
      .addItem({
        productId: product.productId,
        quantity: 1,
      })
      .subscribe({
        next: () => {
          this.addingToCart.set(false);
          this.cartMessage.set('Added to cart');
        },
        error: () => {
          this.addingToCart.set(false);
          this.cartMessage.set('Failed to add to cart');
        },
      });
  }
}
