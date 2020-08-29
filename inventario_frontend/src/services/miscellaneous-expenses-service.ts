import { apiClient } from "./apiClient";
import {
  MiscellaneousExpenses,
  MiscellaneousExpensesForCreation
} from "@/models/miscellaneousExpenses";

export interface MiscellaneousExpensesService {
  getMiscellaneousExpenses(): Promise<MiscellaneousExpenses[]>;
  getMiscellaneousExpense(id: string): Promise<MiscellaneousExpenses>;
  addMiscellaneousExpenses(
    miscellaneousExpenses: MiscellaneousExpensesForCreation
  ): Promise<MiscellaneousExpenses>;
  updateMiscellaneousExpenses(
    miscellaneousExpenses: MiscellaneousExpenses
  ): Promise<void>;
  deleteMiscellaneousExpenses(miscellaneousExpensesId: string): Promise<void>;
}

export class NavigatorMiscellaneousExpensesService
  implements MiscellaneousExpensesService {
  public async getMiscellaneousExpenses(): Promise<MiscellaneousExpenses[]> {
    return await (await apiClient.get("/miscellaneous-expenses")).data;
  }

  public async getMiscellaneousExpense(
    id: string
  ): Promise<MiscellaneousExpenses> {
    return (await apiClient.get("/miscellaneous-expenses/" + id)).data;
  }

  public addMiscellaneousExpenses(
    miscellaneousExpenses: MiscellaneousExpensesForCreation
  ): Promise<MiscellaneousExpenses> {
    return apiClient.post("/miscellaneous-expenses", miscellaneousExpenses);
  }

  public updateMiscellaneousExpenses(
    miscellaneousExpenses: MiscellaneousExpenses
  ): Promise<void> {
    return apiClient.put(
      "/miscellaneous-expenses/" + miscellaneousExpenses.id,
      miscellaneousExpenses
    );
  }

  public deleteMiscellaneousExpenses(
    miscellaneousExpensesId: string
  ): Promise<void> {
    return apiClient.delete(
      "/miscellaneous-expenses/" + miscellaneousExpensesId
    );
  }
}
