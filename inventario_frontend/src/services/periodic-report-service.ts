import { apiClient } from "./apiClient";
import { PeriodicReport } from "@/models/periodicReport";

export interface PeriodicReportService {
  getByYear(year: number): Promise<PeriodicReport[]>;
}

export class NavigatorPeriodicReportService implements PeriodicReportService {
  public async getByYear(year: number): Promise<PeriodicReport[]> {
    console.log(year);
    return await (await apiClient.get("/periodic-reports/" + year)).data;
  }
}
