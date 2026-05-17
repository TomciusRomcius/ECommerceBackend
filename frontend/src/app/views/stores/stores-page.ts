import { Component, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { ActivatedRoute } from '@angular/router';
import { PagePadder } from '../../components/page-padder/page-padder';
import StoreLocationModel from '../../models/store-location-model';

@Component({
  selector: 'app-stores-page',
  imports: [MatCardModule, PagePadder],
  templateUrl: './stores-page.html',
})
export class StoresPage {
  private readonly activatedRoute = inject(ActivatedRoute);
  storeLocations = signal<StoreLocationModel[]>([]);

  constructor() {
    this.activatedRoute.data.subscribe(({ storeLocations }) => {
      this.storeLocations.set(storeLocations);
    });
  }
}
