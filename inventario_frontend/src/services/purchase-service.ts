import { apiClient } from "./apiClient";
import { Purchase, PurchaseForCreation } from "@/models/purchase";
// import { PurchaseFilters } from "@/models/purchaseFilters";

export interface PurchaseService {
  getPurchases(): Promise<Purchase[]>;
  // getPurchasesFiltered(purchaseFilters: PurchaseFilters): Promise<Purchase[]>;
  getPurchase(id: string): Promise<Purchase>;
  addPurchase(purchase: PurchaseForCreation): Promise<Purchase>;
  updatePurchase(purchase: Purchase): Promise<void>;
  deletePurchase(purchaseId: string): Promise<void>;
}

export class NavigatorPurchaseService implements PurchaseService {
  public async getPurchases(): Promise<Purchase[]> {
    return await (await apiClient.get("/purchases")).data;
  }

  // public async getPurchasesFiltered(
  //   purchaseFilters: PurchaseFilters
  // ): Promise<Purchase[]> {
  //   return (
  //     await apiClient.get(
  //       "/purchases/Filtered?" + this.getQueryString(purchaseFilters)
  //     )
  //   ).data;
  // }

  public async getPurchase(id: string): Promise<Purchase> {
    return (await apiClient.get("/purchases/" + id)).data;
  }

  public addPurchase(purchase: PurchaseForCreation): Promise<Purchase> {
    return apiClient.post("/purchases", purchase);
  }

  public updatePurchase(purchase: Purchase): Promise<void> {
    return apiClient.put("/purchases/" + purchase.id, purchase);
  }

  public deletePurchase(purchaseId: string): Promise<void> {
    return apiClient.delete("/purchases/" + purchaseId);
  }

  // public getQueryString(purchaseFilters: PurchaseFilters): string {
  //   const queryString = Object.keys(purchaseFilters).map(key =>
  //     purchaseFilters[key as keyof PurchaseFilters]
  //       ? key + "=" + purchaseFilters[key as keyof PurchaseFilters]
  //       : null
  //   );
  //   return queryString.filter(x => x !== null).join("&");
  // }
}
