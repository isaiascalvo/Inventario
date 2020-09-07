import { apiClient } from "./apiClient";
import { Category, CategoryForCreation } from "@/models/category";

export interface CategoryService {
  getCategories(): Promise<Category[]>;
  getCategory(id: string): Promise<Category>;
  addCategory(category: CategoryForCreation): Promise<Category>;
  updateCategory(category: Category): Promise<void>;
  deleteCategory(categoryId: string): Promise<void>;
  getTotalQty(): Promise<number>;
  getByPageAndQty(skip: number, qty: number): Promise<Category[]>;
}

export class NavigatorCategoryService implements CategoryService {
  public async getCategories(): Promise<Category[]> {
    return await (await apiClient.get("/categories")).data;
  }

  public async getTotalQty(): Promise<number> {
    return (await apiClient.get("/categories/GetTotalQty")).data;
  }

  public async getByPageAndQty(skip: number, qty: number): Promise<Category[]> {
    return (
      await apiClient.get(
        "/categories/GetByPageAndQty?skip=" + skip + "&qty=" + qty
      )
    ).data;
  }

  public async getCategory(id: string): Promise<Category> {
    return (await apiClient.get("/categories/" + id)).data;
  }

  public addCategory(category: CategoryForCreation): Promise<Category> {
    return apiClient.post("/categories", category);
  }

  public updateCategory(category: Category): Promise<void> {
    return apiClient.put("/categories/" + category.id, category);
  }

  public deleteCategory(categoryId: string): Promise<void> {
    return apiClient.delete("/categories/" + categoryId);
  }
}
