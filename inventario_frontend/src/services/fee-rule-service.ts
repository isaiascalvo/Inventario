import { apiClient } from "./apiClient";
import { FeeRule, FeeRuleForCreation } from "@/models/feeRule";
import { FeeRuleByCategory } from "@/models/feeRuleByCategory";
import { FeeRuleFilters } from "@/models/filters/feeRuleFilters";

export interface FeeRuleService {
  getFeeRules(): Promise<FeeRule[]>;
  getFeeRule(id: string): Promise<FeeRule>;
  addFeeRule(feeRule: FeeRuleForCreation): Promise<FeeRule>;
  updateFeeRule(feeRule: FeeRule): Promise<void>;
  deleteFeeRule(feeRuleId: string): Promise<void>;
  addFeeRuleByCategory(feeRuleByCategory: FeeRuleByCategory): Promise<void>;
  getFeeRulesByProducts(productsIds: string[]): Promise<FeeRule[]>;
  getTotalQty(): Promise<number>;
  getTotalQtyByFilters(feeRuleFilters: FeeRuleFilters): Promise<number>;
  getByPageAndQty(skip: number, qty: number): Promise<FeeRule[]>;
  getByFiltersPageAndQty(
    feeRuleFilters: FeeRuleFilters,
    skip: number,
    qty: number
  ): Promise<FeeRule[]>;
}

export class NavigatorFeeRuleService implements FeeRuleService {
  public async getTotalQty(): Promise<number> {
    return (await apiClient.get("/fee-rules/GetTotalQty")).data;
  }

  public async getTotalQtyByFilters(
    feeRuleFilters: FeeRuleFilters
  ): Promise<number> {
    return (
      await apiClient.get(
        "/fee-rules/GetTotalQtyByFilters?" + this.getQueryString(feeRuleFilters)
      )
    ).data;
  }

  public async getByPageAndQty(skip: number, qty: number): Promise<FeeRule[]> {
    return (
      await apiClient.get(
        "/fee-rules/GetByPageAndQty?skip=" + skip + "&qty=" + qty
      )
    ).data;
  }

  public async getByFiltersPageAndQty(
    feeRuleFilters: FeeRuleFilters,
    skip: number,
    qty: number
  ): Promise<FeeRule[]> {
    return (
      await apiClient.get(
        "/fee-rules/GetByFiltersPageAndQty?" +
          this.getQueryString(feeRuleFilters) +
          "&skip=" +
          skip +
          "&qty=" +
          qty
      )
    ).data;
  }

  public async getFeeRules(): Promise<FeeRule[]> {
    return await (await apiClient.get("/fee-rules")).data;
  }

  public async getFeeRule(id: string): Promise<FeeRule> {
    return (await apiClient.get("/fee-rules/" + id)).data;
  }

  public addFeeRule(feeRule: FeeRuleForCreation): Promise<FeeRule> {
    return apiClient.post("/fee-rules", feeRule);
  }

  public updateFeeRule(feeRule: FeeRule): Promise<void> {
    return apiClient.put("/fee-rules/" + feeRule.id, feeRule);
  }

  public deleteFeeRule(feeRuleId: string): Promise<void> {
    return apiClient.delete("/fee-rules/" + feeRuleId);
  }

  public addFeeRuleByCategory(
    feeRuleByCategory: FeeRuleByCategory
  ): Promise<void> {
    // console.log("Llamó", JSON.parse(JSON.stringify(feeRuleByCategory)));
    return apiClient.post("/fee-rules/ByCategory", feeRuleByCategory);
  }

  public async getFeeRulesByProducts(
    productsIds: string[]
  ): Promise<FeeRule[]> {
    return await (await apiClient.post("/fee-rules/ByProducts", productsIds))
      .data;
  }

  public getQueryString(feeRuleFilters: FeeRuleFilters): string {
    const queryString = Object.keys(feeRuleFilters).map(key =>
      feeRuleFilters[key as keyof FeeRuleFilters]
        ? key + "=" + feeRuleFilters[key as keyof FeeRuleFilters]
        : null
    );
    return queryString.filter(x => x !== null).join("&");
  }
}
