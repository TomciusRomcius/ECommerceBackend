import { CurrencyPipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Paginator } from '../../../components/paginator/paginator';
import PageModel, { emptyPage } from '../../../models/page-model';
import ProductModel from '../../../models/product-model';
import { SearchBar } from "@components/search-bar/search-bar";

@Component({
  selector: 'app-products',
  imports: [CurrencyPipe, MatButtonModule, MatTableModule, Paginator, RouterLink, SearchBar],
  templateUrl: './products.html',
  styleUrl: './products.css',
})
export class Products {
  private route = inject(ActivatedRoute);
  products = signal<PageModel<ProductModel>>(
    this.route.snapshot.data['products'] ?? emptyPage(),
  );
  columnsToDisplay = ['name', 'price', 'actions'];

  constructor() {
    this.route.data.subscribe((data) => {
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
