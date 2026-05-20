import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import CreateProductRequest from '../models/create-product-request';
import CreateProductResponse from '../models/create-product-response';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ProductAdminService {
  private readonly http = inject(HttpClient);

  createProduct(request: CreateProductRequest): Observable<CreateProductResponse> {
    return this.http.post<CreateProductResponse>(
      `${environment.backendApi}/Products`,
      request,
    );
  }
}
