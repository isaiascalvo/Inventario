import { apiClient } from "./apiClient";
import { ProductEntryLine, ProductEntryLineForCreation } from "@/models/productEntryLine";

export interface ProductEntryLineService {
  getProductEntryLines(): Promise<ProductEntryLine[]>;
  getProductEntryLine(id: string): Promise<ProductEntryLine>;
  addProductEntryLine(productEntryLine: ProductEntryLineForCreation): Promise<ProductEntryLine>;
  updateProductEntryLine(productEntryLine: ProductEntryLine): Promise<void>;
  deleteProductEntryLine(productEntryLineId: string): Promise<void>;
}

export class NavigatorProductEntryLineService implements ProductEntryLineService {
  public async getProductEntryLines(): Promise<ProductEntryLine[]> {
    return await (await apiClient.get("/product-entry-lines")).data;
  }

  public async getProductEntryLine(id: string): Promise<ProductEntryLine> {
    return (await apiClient.get("/product-entry-lines/" + id)).data;
  }

  public addProductEntryLine(productEntryLine: ProductEntryLineForCreation): Promise<ProductEntryLine> {
    return apiClient.post("/product-entry-lines", productEntryLine);
  }

  public updateProductEntryLine(productEntryLine: ProductEntryLine): Promise<void> {
    return apiClient.put("/product-entry-lines/" + productEntryLine.id, productEntryLine);
  }

  public deleteProductEntryLine(productEntryLineId: string): Promise<void> {
    return apiClient.delete("/product-entry-lines/" + productEntryLineId);
  }
}
