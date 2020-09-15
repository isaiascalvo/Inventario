import { apiClient } from "./apiClient";
import { Commission, CommissionForCreation } from "@/models/commission";
import { CommissionFilters } from "@/models/filters/commissionFilters";

export interface CommissionService {
  getCommissions(): Promise<Commission[]>;
  getCommission(id: string): Promise<Commission>;
  addCommission(commission: CommissionForCreation): Promise<Commission>;
  updateCommission(commission: Commission): Promise<void>;
  deleteCommission(commissionId: string): Promise<void>;
  // getTotalQty(): Promise<number>;
  getTotalQtyByFilters(commissionFilters: CommissionFilters): Promise<number>;
  // getByPageAndQty(skip: number, qty: number): Promise<Commission[]>;
  getByFiltersPageAndQty(
    commissionFilters: CommissionFilters,
    skip: number,
    qty: number
  ): Promise<Commission[]>;
}

export class NavigatorCommissionService implements CommissionService {
  // public async getTotalQty(): Promise<number> {
  //   return (await apiClient.get("/commissions/GetTotalQty")).data;
  // }

  public async getTotalQtyByFilters(
    commissionFilters: CommissionFilters
  ): Promise<number> {
    return (
      await apiClient.get(
        "/commissions/GetTotalQtyByFilters?" +
          this.getQueryString(commissionFilters)
      )
    ).data;
  }

  // public async getByPageAndQty(skip: number, qty: number): Promise<Commission[]> {
  //   return (
  //     await apiClient.get(
  //       "/commissions/GetByPageAndQty?skip=" + skip + "&qty=" + qty
  //     )
  //   ).data;
  // }

  public async getByFiltersPageAndQty(
    commissionFilters: CommissionFilters,
    skip: number,
    qty: number
  ): Promise<Commission[]> {
    return (
      await apiClient.get(
        "/commissions/GetByFiltersPageAndQty?" +
          this.getQueryString(commissionFilters) +
          "&skip=" +
          skip +
          "&qty=" +
          qty
      )
    ).data;
  }

  public async getCommissions(): Promise<Commission[]> {
    return await (await apiClient.get("/commissions")).data;
  }

  public async getCommission(id: string): Promise<Commission> {
    return (await apiClient.get("/commissions/" + id)).data;
  }

  public addCommission(commission: CommissionForCreation): Promise<Commission> {
    return apiClient.post("/commissions", commission);
  }

  public updateCommission(commission: Commission): Promise<void> {
    return apiClient.put("/commissions/" + commission.id, commission);
  }

  public deleteCommission(commissionId: string): Promise<void> {
    return apiClient.delete("/commissions/" + commissionId);
  }

  public getQueryString(commissionFilters: CommissionFilters): string {
    commissionFilters.dateFrom = commissionFilters.dateDateFrom?.toISOString();
    commissionFilters.dateTo = commissionFilters.dateDateTo?.toISOString();
    const queryString = Object.keys(commissionFilters).map(key =>
      commissionFilters[key as keyof CommissionFilters] !== undefined &&
      commissionFilters[key as keyof CommissionFilters] !== null &&
      key !== "dateDateFrom" &&
      key !== "dateDateTo"
        ? key + "=" + commissionFilters[key as keyof CommissionFilters]
        : null
    );
    return queryString.filter(x => x !== null).join("&");
  }
}
