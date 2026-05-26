import { CurrencyPipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { ActivatedRoute, RouterLink } from '@angular/router';
import ProductModel from '../../../models/product-model';

@Component({
  selector: 'app-products',
  imports: [CurrencyPipe, MatButtonModule, MatTableModule, RouterLink],
  templateUrl: './products.html',
  styleUrl: './products.css',
})
export class Products {
  private route = inject(ActivatedRoute);
  products = signal<ProductModel[]>(this.route.snapshot.data['products'] ?? []);
  columnsToDisplay = ['name', 'price', 'actions'];

  constructor() {
    this.route.data.subscribe((data) => {
      this.products.set(data['products'] as ProductModel[]);
    });
  }

  edit(product: ProductModel): void {
    console.log('edit', product);
  }

  delete(product: ProductModel): void {
    console.log('delete', product);
  }
}
