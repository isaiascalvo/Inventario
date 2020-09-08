import { apiClient } from "./apiClient";
import { ProductEntry, ProductEntryForCreation } from "@/models/productEntry";
import { ProductEntryFilters } from "@/models/filters/productEntryFilters";

export interface ProductEntryService {
  getProductEntries(): Promise<ProductEntry[]>;
  getProductEntry(id: string): Promise<ProductEntry>;
  addProductEntry(productEntry: ProductEntryForCreation): Promise<ProductEntry>;
  updateProductEntry(productEntry: ProductEntry): Promise<void>;
  deleteProductEntry(productEntryId: string): Promise<void>;
  getTotalQty(): Promise<number>;
  getTotalQtyByFilters(
    productEntryFilters: ProductEntryFilters
  ): Promise<number>;
  getByPageAndQty(skip: number, qty: number): Promise<ProductEntry[]>;
  getByFiltersPageAndQty(
    productEntryFilters: ProductEntryFilters,
    skip: number,
    qty: number
  ): Promise<ProductEntry[]>;
}

export class NavigatorProductEntryService implements ProductEntryService {
  public async getTotalQty(): Promise<number> {
    return (await apiClient.get("/product-entries/GetTotalQty")).data;
  }

  public async getTotalQtyByFilters(
    productEntryFilters: ProductEntryFilters
  ): Promise<number> {
    return (
      await apiClient.get(
        "/product-entries/GetTotalQtyByFilters?" +
          this.getQueryString(productEntryFilters)
      )
    ).data;
  }

  public async getByPageAndQty(
    skip: number,
    qty: number
  ): Promise<ProductEntry[]> {
    return (
      await apiClient.get(
        "/product-entries/GetByPageAndQty?skip=" + skip + "&qty=" + qty
      )
    ).data;
  }

  public async getByFiltersPageAndQty(
    productEntryFilters: ProductEntryFilters,
    skip: number,
    qty: number
  ): Promise<ProductEntry[]> {
    return (
      await apiClient.get(
        "/product-entries/GetByFiltersPageAndQty?" +
          this.getQueryString(productEntryFilters) +
          "&skip=" +
          skip +
          "&qty=" +
          qty
      )
    ).data;
  }

  public async getProductEntries(): Promise<ProductEntry[]> {
    return await (await apiClient.get("/product-entries")).data;
  }

  public async getProductEntry(id: string): Promise<ProductEntry> {
    return (await apiClient.get("/product-entries/" + id)).data;
  }

  public addProductEntry(
    productEntry: ProductEntryForCreation
  ): Promise<ProductEntry> {
    return apiClient.post("/product-entries", productEntry);
  }

  public updateProductEntry(productEntry: ProductEntry): Promise<void> {
    return apiClient.put("/product-entries/" + productEntry.id, productEntry);
  }

  public deleteProductEntry(productEntryId: string): Promise<void> {
    return apiClient.delete("/product-entries/" + productEntryId);
  }

  public getQueryString(productEntryFilters: ProductEntryFilters): string {
    productEntryFilters.dateFrom = productEntryFilters.dateDateFrom?.toISOString();
    productEntryFilters.dateTo = productEntryFilters.dateDateTo?.toISOString();
    const queryString = Object.keys(productEntryFilters).map(key =>
      productEntryFilters[key as keyof ProductEntryFilters] &&
      key !== "dateDateFrom" &&
      key !== "dateDateTo"
        ? key + "=" + productEntryFilters[key as keyof ProductEntryFilters]
        : null
    );
    return queryString.filter(x => x !== null).join("&");
  }
}
