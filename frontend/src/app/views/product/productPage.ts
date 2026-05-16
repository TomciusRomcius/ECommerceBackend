import { CurrencyPipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute } from '@angular/router';
import { PagePadder } from '../../components/page-padder/page-padder';
import ProductModel from '../../models/product-model';

@Component({
  selector: 'app-product-page',
  imports: [CurrencyPipe, MatButtonModule, PagePadder],
  templateUrl: './product-page.html',
})
export class ProductPage {
  private readonly route = inject(ActivatedRoute);

  product = signal<ProductModel | null>(null);

  constructor() {
    this.route.data.subscribe(({ product }) => {
      this.product.set(product);
    });
  }
}
