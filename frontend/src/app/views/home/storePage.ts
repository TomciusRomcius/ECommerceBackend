import { CurrencyPipe } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { PagePadder } from "../../components/page-padder/page-padder";
import { ActivatedRoute, RouterLink } from '@angular/router';
import ProductModel from '../../models/product-model';

@Component({
  selector: 'app-home',
  imports: [CurrencyPipe, MatButtonModule, MatCardModule, PagePadder, RouterLink],
  templateUrl: './store-page.html',
})
export class StorePage {
  private readonly activatedRoute = inject(ActivatedRoute);

  products = signal<ProductModel[]>([]);
  storeLocationId = signal<number | null>(null);

  isStoreFiltered = computed(() => this.storeLocationId() !== null);

  storeHeading = computed(() => {
    if (!this.isStoreFiltered()) {
      return null;
    }

    return this.products()[0]?.store?.displayName ?? `Store #${this.storeLocationId()}`;
  });

  storeAddress = computed(() => {
    if (!this.isStoreFiltered()) {
      return null;
    }

    return this.products()[0]?.store?.address ?? null;
  });

  constructor() {
    this.activatedRoute.queryParamMap.subscribe((params) => {
      const id = params.get('storeLocationId');
      this.storeLocationId.set(id ? Number(id) : null);
    });

    this.activatedRoute.data.subscribe(({ products }) => {
      this.products.set(products);
    });
  }
}
