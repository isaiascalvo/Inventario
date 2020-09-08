import { apiClient } from "./apiClient";
import {
  MiscellaneousExpenses,
  MiscellaneousExpensesForCreation
} from "@/models/miscellaneousExpenses";
import { MiscellaneousExpensesFilters } from "@/models/filters/miscellaneousExpensesFilters";

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
  getTotalQty(): Promise<number>;
  getTotalQtyByFilters(
    miscellaneousExpensesFilters: MiscellaneousExpensesFilters
  ): Promise<number>;
  getByPageAndQty(skip: number, qty: number): Promise<MiscellaneousExpenses[]>;
  getByFiltersPageAndQty(
    miscellaneousExpensesFilters: MiscellaneousExpensesFilters,
    skip: number,
    qty: number
  ): Promise<MiscellaneousExpenses[]>;
}

export class NavigatorMiscellaneousExpensesService
  implements MiscellaneousExpensesService {
  public async getTotalQty(): Promise<number> {
    return (await apiClient.get("/miscellaneous-expenses/GetTotalQty")).data;
  }

  public async getTotalQtyByFilters(
    miscellaneousExpensesFilters: MiscellaneousExpensesFilters
  ): Promise<number> {
    return (
      await apiClient.get(
        "/miscellaneous-expenses/GetTotalQtyByFilters?" +
          this.getQueryString(miscellaneousExpensesFilters)
      )
    ).data;
  }

  public async getByPageAndQty(
    skip: number,
    qty: number
  ): Promise<MiscellaneousExpenses[]> {
    return (
      await apiClient.get(
        "/miscellaneous-expenses/GetByPageAndQty?skip=" + skip + "&qty=" + qty
      )
    ).data;
  }

  async getByFiltersPageAndQty(
    miscellaneousExpensesFilters: MiscellaneousExpensesFilters,
    skip: number,
    qty: number
  ): Promise<MiscellaneousExpenses[]> {
    return (
      await apiClient.get(
        "/miscellaneous-expenses/GetByFiltersPageAndQty?" +
          this.getQueryString(miscellaneousExpensesFilters) +
          "&skip=" +
          skip +
          "&qty=" +
          qty
      )
    ).data;
  }

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

  public getQueryString(
    miscellaneousExpensesFilters: MiscellaneousExpensesFilters
  ): string {
    miscellaneousExpensesFilters.dateFrom = miscellaneousExpensesFilters.dateDateFrom?.toISOString();
    miscellaneousExpensesFilters.dateTo = miscellaneousExpensesFilters.dateDateTo?.toISOString();
    const queryString = Object.keys(miscellaneousExpensesFilters).map(key =>
      miscellaneousExpensesFilters[
        key as keyof MiscellaneousExpensesFilters
      ] !== undefined &&
      miscellaneousExpensesFilters[
        key as keyof MiscellaneousExpensesFilters
      ] !== null &&
      key !== "dateDateFrom" &&
      key !== "dateDateTo"
        ? key +
          "=" +
          miscellaneousExpensesFilters[
            key as keyof MiscellaneousExpensesFilters
          ]
        : null
    );
    return queryString.filter(x => x !== null).join("&");
  }
}
