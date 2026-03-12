import { ActivatedRouteSnapshot, MaybeAsync, RedirectCommand, Resolve, RouterStateSnapshot } from "@angular/router";
import ProductModel from "../../models/product-model";
import { inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { Observable } from "rxjs";

export class ProductResolver implements Resolve<ProductModel[]> {
  httpClient = inject(HttpClient);

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<ProductModel[]> {
    return this.httpClient.get<ProductModel[]>(`${environment.backendApi}/storeproducts`);
  }
}
