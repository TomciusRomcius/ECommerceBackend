import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import CreateManufacturerRequest from '../models/create-manufacturer-request';
import CreateManufacturerResponse from '../models/create-manufacturer-response';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ManufacturerService {
  private readonly http = inject(HttpClient);

  createManufacturer(request: CreateManufacturerRequest): Observable<CreateManufacturerResponse> {
    return this.http.post<CreateManufacturerResponse>(
      `${environment.backendApi}/Manufacturer`,
      request,
    );
  }
}
