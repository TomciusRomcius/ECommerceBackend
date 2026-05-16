import { ActivatedRouteSnapshot, ResolveFn, RouterStateSnapshot } from "@angular/router";
import ProductModel from "../../models/product-model";
import { inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment";

export const productResolver: ResolveFn<ProductModel[]> = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot,
) => {
  const httpClient = inject(HttpClient);
  return httpClient.get<ProductModel[]>(`${environment.backendApi}/storeproducts`);
}
