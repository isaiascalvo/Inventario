import { apiClient } from "./apiClient";
import { Sale, SaleForCreation } from "@/models/sale";
import { SaleFilters } from "@/models/filters/saleFilters";

export interface SaleService {
  getSales(): Promise<Sale[]>;
  getSale(id: string): Promise<Sale>;
  preCreation(sale: SaleForCreation): Promise<Sale>;
  addSale(sale: SaleForCreation): Promise<Sale>;
  // updateSale(sale: Sale): Promise<void>;
  deleteSale(saleId: string): Promise<void>;
  getTotalQty(): Promise<number>;
  getTotalQtyByFilters(saleFilters: SaleFilters): Promise<number>;
  getByPageAndQty(skip: number, qty: number): Promise<Sale[]>;
  getByFiltersPageAndQty(
    saleFilters: SaleFilters,
    skip: number,
    qty: number
  ): Promise<Sale[]>;
}

export class NavigatorSaleService implements SaleService {
  public async getTotalQty(): Promise<number> {
    return (await apiClient.get("/sales/GetTotalQty")).data;
  }

  public async getTotalQtyByFilters(saleFilters: SaleFilters): Promise<number> {
    return (
      await apiClient.get(
        "/sales/GetTotalQtyByFilters?" + this.getQueryString(saleFilters)
      )
    ).data;
  }

  public async getByPageAndQty(skip: number, qty: number): Promise<Sale[]> {
    return (
      await apiClient.get("/sales/GetByPageAndQty?skip=" + skip + "&qty=" + qty)
    ).data;
  }

  async getByFiltersPageAndQty(
    saleFilters: SaleFilters,
    skip: number,
    qty: number
  ): Promise<Sale[]> {
    return (
      await apiClient.get(
        "/sales/GetByFiltersPageAndQty?" +
          this.getQueryString(saleFilters) +
          "&skip=" +
          skip +
          "&qty=" +
          qty
      )
    ).data;
  }

  public async getSales(): Promise<Sale[]> {
    return await (await apiClient.get("/sales")).data;
  }

  public async getSale(id: string): Promise<Sale> {
    return (await apiClient.get("/sales/" + id)).data;
  }

  public async preCreation(sale: SaleForCreation): Promise<Sale> {
    return (await apiClient.post("/sales/PreCreation", sale)).data;
  }

  public addSale(sale: SaleForCreation): Promise<Sale> {
    return apiClient.post("/sales", sale);
  }

  // public updateSale(sale: Sale): Promise<void> {
  //   return apiClient.put("/sales/" + sale.id, sale);
  // }

  public deleteSale(saleId: string): Promise<void> {
    return apiClient.delete("/sales/" + saleId);
  }

  public async payFee(feeId: string, paymentDate: string): Promise<void> {
    return (
      await apiClient.get(
        "/sales/PayFee?feeId=" + feeId + "&paymentDate=" + paymentDate
      )
    ).data;
  }

  public getQueryString(saleFilters: SaleFilters): string {
    saleFilters.dateFrom = saleFilters.dateDateFrom?.toISOString();
    saleFilters.dateTo = saleFilters.dateDateTo?.toISOString();
    const queryString = Object.keys(saleFilters).map(key =>
      saleFilters[key as keyof SaleFilters] !== undefined &&
      saleFilters[key as keyof SaleFilters] !== null &&
      key !== "dateDateFrom" &&
      key !== "dateDateTo"
        ? key + "=" + saleFilters[key as keyof SaleFilters]
        : null
    );
    return queryString.filter(x => x !== null).join("&");
  }
}
