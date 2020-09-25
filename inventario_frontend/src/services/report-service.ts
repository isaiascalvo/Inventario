import { apiClient } from "./apiClient";
import { PeriodicReport } from "@/models/reports/periodicReport";
import { FeesReport } from "@/models/reports/feesReport";
import { FeesReportFilters } from "@/models/filters/feesReportFilters";

export interface ReportService {
  getAnnualReport(year: number): Promise<PeriodicReport[]>;
  getFeesReport(feesReportFilters: FeesReportFilters): Promise<FeesReport[]>;
}

export class NavigatorReportService implements ReportService {
  public async getAnnualReport(year: number): Promise<PeriodicReport[]> {
    return await (await apiClient.get("/reports/AnnualReport/" + year)).data;
  }

  public async getFeesReport(
    feesReportFilters: FeesReportFilters
  ): Promise<FeesReport[]> {
    return await (
      await apiClient.get(
        "/reports/FeesReport?" + this.getQueryString(feesReportFilters)
      )
    ).data;
  }

  public getQueryString(feesReportFilters: FeesReportFilters): string {
    feesReportFilters.dateFrom = feesReportFilters.dateDateFrom?.toISOString();
    feesReportFilters.dateTo = feesReportFilters.dateDateTo?.toISOString();
    const queryString = Object.keys(feesReportFilters).map(key =>
      feesReportFilters[key as keyof FeesReportFilters] !== undefined &&
      feesReportFilters[key as keyof FeesReportFilters] !== null &&
      key !== "dateDateFrom" &&
      key !== "dateDateTo"
        ? key + "=" + feesReportFilters[key as keyof FeesReportFilters]
        : null
    );
    return queryString.filter(x => x !== null).join("&");
  }
}
