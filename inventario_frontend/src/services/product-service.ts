import { Product, ProductForCreation } from "@/models/product";
import { ProductFilters } from "@/models/productFilters";
import { apiClient } from "./apiClient";

export interface ProductService {
  getProducts(): Promise<Product[]>;
  getProductsFiltered(productFilters: ProductFilters): Promise<Product[]>;
  getOneProductByFilters(productFilters: ProductFilters): Promise<Product>;
  getProduct(id: string): Promise<Product>;
  addProduct(product: ProductForCreation): Promise<Product>;
  updateProduct(product: Product): Promise<void>;
  deleteProduct(productId: string): Promise<void>;
  getPriceByDate(productId: string, date: string): Promise<number>;
}

export class NavigatorProductService implements ProductService {
  public async getProducts(): Promise<Product[]> {
    return (await apiClient.get("/products")).data;
  }

  public async getProductsFiltered(
    productFilters: ProductFilters
  ): Promise<Product[]> {
    return (
      await apiClient.get(
        "/products/Filtered?" + this.getQueryString(productFilters)
      )
    ).data;
  }

  public async getOneProductByFilters(
    productFilters: ProductFilters
  ): Promise<Product> {
    return (
      await apiClient.get(
        "/products/GetOneByFilters?" + this.getQueryString(productFilters)
      )
    ).data;
  }

  public async getProduct(id: string): Promise<Product> {
    return (await apiClient.get("/products/" + id)).data;
  }

  public addProduct(product: ProductForCreation): Promise<Product> {
    return apiClient.post("/products", product);
  }

  public updateProduct(product: Product): Promise<void> {
    return apiClient.put("/products/" + product.id, product);
  }

  public deleteProduct(productId: string): Promise<void> {
    return apiClient.delete("/products/" + productId);
  }

  public async getPriceByDate(
    productId: string,
    date: string
  ): Promise<number> {
    return (
      await apiClient.get(
        "/products/GetPrice?productId=" + productId + "&date=" + date
      )
    ).data;
  }

  public getQueryString(productFilters: ProductFilters): string {
    const queryString = Object.keys(productFilters).map(key =>
      productFilters[key as keyof ProductFilters]
        ? key + "=" + productFilters[key as keyof ProductFilters]
        : null
    );
    return queryString.filter(x => x !== null).join("&");
  }
}
