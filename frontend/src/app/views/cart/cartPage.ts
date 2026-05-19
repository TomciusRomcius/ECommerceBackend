import { CurrencyPipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { Router, RouterLink } from '@angular/router';
import { PagePadder } from '../../components/page-padder/page-padder';
import { ACCESS_TOKEN_KEY } from '../../constants/auth';
import CartItemModel from '../../models/cart-item-model';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-cart-page',
  imports: [CurrencyPipe, MatButtonModule, MatCardModule, PagePadder, RouterLink],
  templateUrl: './cart-page.html',
})
export class CartPage implements OnInit {
  private readonly router = inject(Router);
  private readonly cartService = inject(CartService);

  items = signal<CartItemModel[]>([]);
  error = signal<string | null>(null);
  loading = signal(true);
  removingProductId = signal<number | null>(null);

  ngOnInit(): void {
    if (!sessionStorage.getItem(ACCESS_TOKEN_KEY)) {
      void this.router.navigate(['/login']);
      return;
    }

    this.cartService.getItems().subscribe({
      next: (items) => {
        this.items.set(items);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Failed to load cart items. Please try again.');
        this.loading.set(false);
      },
    });
  }

  removeItem(item: CartItemModel): void {
    this.removingProductId.set(item.productId);

    this.cartService.removeItem(item.productId, item.storeLocationId).subscribe({
      next: () => {
        this.items.update((items) =>
          items.filter((i) => !(i.productId === item.productId && i.storeLocationId === item.storeLocationId)),
        );
        this.removingProductId.set(null);
      },
      error: () => {
        this.error.set('Failed to remove item from cart.');
        this.removingProductId.set(null);
      },
    });
  }
}
