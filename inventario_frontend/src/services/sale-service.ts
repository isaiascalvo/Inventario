import { apiClient } from "./apiClient";
import { Sale, SaleForCreation } from "@/models/sale";
// import { SaleFilters } from "@/models/saleFilters";

export interface SaleService {
  getSales(): Promise<Sale[]>;
  // getSalesFiltered(saleFilters: SaleFilters): Promise<Sale[]>;
  getSale(id: string): Promise<Sale>;
  addSale(sale: SaleForCreation): Promise<Sale>;
  // updateSale(sale: Sale): Promise<void>;
  deleteSale(saleId: string): Promise<void>;
}

export class NavigatorSaleService implements SaleService {
  public async getSales(): Promise<Sale[]> {
    return await (await apiClient.get("/sales")).data;
  }

  // public async getSalesFiltered(
  //   saleFilters: SaleFilters
  // ): Promise<Sale[]> {
  //   return (
  //     await apiClient.get(
  //       "/sales/Filtered?" + this.getQueryString(saleFilters)
  //     )
  //   ).data;
  // }

  public async getSale(id: string): Promise<Sale> {
    return (await apiClient.get("/sales/" + id)).data;
  }

  public addSale(sale: SaleForCreation): Promise<Sale> {
    console.log(JSON.parse(JSON.stringify(sale)));
    return apiClient.post("/sales", sale);
  }

  // public updateSale(sale: Sale): Promise<void> {
  //   return apiClient.put("/sales/" + sale.id, sale);
  // }

  public deleteSale(saleId: string): Promise<void> {
    return apiClient.delete("/sales/" + saleId);
  }

  // public getQueryString(saleFilters: SaleFilters): string {
  //   const queryString = Object.keys(saleFilters).map(key =>
  //     saleFilters[key as keyof SaleFilters]
  //       ? key + "=" + saleFilters[key as keyof SaleFilters]
  //       : null
  //   );
  //   return queryString.filter(x => x !== null).join("&");
  // }
}
