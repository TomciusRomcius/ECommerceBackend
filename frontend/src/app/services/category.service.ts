import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import CreateCategoryRequest from '../models/create-category-request';
import CreateCategoryResponse from '../models/create-category-response';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class CategoryService {
  private readonly http = inject(HttpClient);

  createCategory(request: CreateCategoryRequest): Observable<CreateCategoryResponse> {
    return this.http.post<CreateCategoryResponse>(
      `${environment.backendApi}/Categories`,
      request,
    );
  }
}
