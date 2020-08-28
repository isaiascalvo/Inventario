import { apiClient } from "./apiClient";
import { FeeRule, FeeRuleForCreation } from "@/models/feeRule";
import { FeeRuleByCategory } from "@/models/feeRuleByCategory";

export interface FeeRuleService {
  getFeeRules(): Promise<FeeRule[]>;
  getFeeRule(id: string): Promise<FeeRule>;
  addFeeRule(feeRule: FeeRuleForCreation): Promise<FeeRule>;
  updateFeeRule(feeRule: FeeRule): Promise<void>;
  deleteFeeRule(feeRuleId: string): Promise<void>;
  addFeeRuleByCategory(feeRuleByCategory: FeeRuleByCategory): Promise<void>;
  getFeeRulesByProduct(productId: string): Promise<FeeRule[]>;
}

export class NavigatorFeeRuleService implements FeeRuleService {
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

  public async getFeeRulesByProduct(productId: string): Promise<FeeRule[]> {
    return await (await apiClient.get("/fee-rules/ByProduct/" + productId))
      .data;
  }
}
