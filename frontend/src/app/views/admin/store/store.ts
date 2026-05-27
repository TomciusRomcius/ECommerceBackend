import { CurrencyPipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Paginator } from '../../../components/paginator/paginator';
import PageModel, { emptyPage } from '../../../models/page-model';
import ProductModel from '../../../models/product-model';
import StoreLocationModel from '../../../models/store-location-model';

@Component({
  selector: 'app-store',
  imports: [CurrencyPipe, MatButtonModule, MatTableModule, Paginator, RouterLink],
  templateUrl: './store.html',
  styleUrl: './store.css',
})
export class Store {
  private route = inject(ActivatedRoute);
  storeLocation = signal<StoreLocationModel | null>(
    this.route.snapshot.data['storeLocation'] ?? null,
  );
  products = signal<PageModel<ProductModel>>(
    this.route.snapshot.data['products'] ?? emptyPage(),
  );
  columnsToDisplay = ['name', 'price', 'stock', 'actions'];

  constructor() {
    this.route.data.subscribe((data) => {
      this.storeLocation.set(data['storeLocation'] as StoreLocationModel);
      this.products.set(data['products'] as PageModel<ProductModel>);
    });
  }

  edit(product: ProductModel): void {
    console.log('edit', product);
  }

  delete(product: ProductModel): void {
    console.log('delete', product);
  }
}
