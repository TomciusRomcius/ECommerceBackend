import { Component, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { PagePadder } from "../../components/page-padder/page-padder";
import { ActivatedRoute } from '@angular/router';
import ProductModel from '../../models/product-model';

@Component({
  selector: 'app-home',
  imports: [MatCardModule, PagePadder],
  templateUrl: './store-page.html',
})
export class StorePage {
  private activedRoute = inject(ActivatedRoute);
  products = signal<ProductModel[]>([]);

  constructor() {
    this.activedRoute.data.subscribe(({products}) => {
      this.products.set(products);
    })
  }
}
