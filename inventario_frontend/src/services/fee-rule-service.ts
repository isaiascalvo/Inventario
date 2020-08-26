import { apiClient } from "./apiClient";
import { FeeRule, FeeRuleForCreation } from "@/models/feeRule";

export interface FeeRuleService {
  getFeeRules(): Promise<FeeRule[]>;
  getFeeRule(id: string): Promise<FeeRule>;
  addFeeRule(feeRule: FeeRuleForCreation): Promise<FeeRule>;
  updateFeeRule(feeRule: FeeRule): Promise<void>;
  deleteFeeRule(feeRuleId: string): Promise<void>;
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
}
