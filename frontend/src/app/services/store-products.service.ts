import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface UpdateProductStockRequest {
  storeLocationId: number;
  productId: number;
  stock: number;
}

@Injectable({ providedIn: 'root' })
export class StoreProductsService {
  private readonly http = inject(HttpClient);

  updateStock(request: UpdateProductStockRequest): Observable<void> {
    return this.http.put<void>(`${environment.backendApi}/storeproducts`, request);
  }
}
