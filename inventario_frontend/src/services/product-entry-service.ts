import { apiClient } from "./apiClient";
import { ProductEntry, ProductEntryForCreation } from "@/models/productEntry";

export interface ProductEntryService {
  getProductEntries(): Promise<ProductEntry[]>;
  getProductEntry(id: string): Promise<ProductEntry>;
  addProductEntry(productEntry: ProductEntryForCreation): Promise<ProductEntry>;
  updateProductEntry(productEntry: ProductEntry): Promise<void>;
  deleteProductEntry(productEntryId: string): Promise<void>;
}

export class NavigatorProductEntryService implements ProductEntryService {
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
}
