import { ActivatedRouteSnapshot, ResolveFn, RouterStateSnapshot } from "@angular/router";
import ProductModel from "../../models/product-model";
import ApiResponse from "../../models/api-response";
import { inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { unwrapApiResponse } from "../../utils/unwrap-api-response";

export const productResolver: ResolveFn<ProductModel[]> = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot,
) => {
  const httpClient = inject(HttpClient);
  return unwrapApiResponse(
    httpClient.get<ApiResponse<ProductModel[]>>(`${environment.backendApi}/storeproducts`),
  );
}
