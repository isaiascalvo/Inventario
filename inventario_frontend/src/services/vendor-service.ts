import { apiClient } from "./apiClient";
import { Vendor, VendorForCreation } from "@/models/vendor";
import { VendorFilters } from "@/models/filters/vendorFilters";

export interface VendorService {
  getVendors(): Promise<Vendor[]>;
  getVendor(id: string): Promise<Vendor>;
  addVendor(vendor: VendorForCreation): Promise<Vendor>;
  updateVendor(vendor: Vendor): Promise<void>;
  deleteVendor(vendorId: string): Promise<void>;
  getTotalQty(): Promise<number>;
  getTotalQtyByFilters(vendorFilters: VendorFilters): Promise<number>;
  getByPageAndQty(skip: number, qty: number): Promise<Vendor[]>;
  getByFiltersPageAndQty(
    vendorFilters: VendorFilters,
    skip: number,
    qty: number
  ): Promise<Vendor[]>;
}

export class NavigatorVendorService implements VendorService {
  public async getTotalQty(): Promise<number> {
    return (await apiClient.get("/vendors/GetTotalQty")).data;
  }

  public async getTotalQtyByFilters(
    vendorFilters: VendorFilters
  ): Promise<number> {
    return (
      await apiClient.get(
        "/vendors/GetTotalQtyByFilters?" + this.getQueryString(vendorFilters)
      )
    ).data;
  }

  public async getByPageAndQty(skip: number, qty: number): Promise<Vendor[]> {
    return (
      await apiClient.get(
        "/vendors/GetByPageAndQty?skip=" + skip + "&qty=" + qty
      )
    ).data;
  }

  async getByFiltersPageAndQty(
    vendorFilters: VendorFilters,
    skip: number,
    qty: number
  ): Promise<Vendor[]> {
    return (
      await apiClient.get(
        "/vendors/GetByFiltersPageAndQty?" +
          this.getQueryString(vendorFilters) +
          "&skip=" +
          skip +
          "&qty=" +
          qty
      )
    ).data;
  }

  public async getVendors(): Promise<Vendor[]> {
    return await (await apiClient.get("/vendors")).data;
  }

  public async getVendor(id: string): Promise<Vendor> {
    return (await apiClient.get("/vendors/" + id)).data;
  }

  public addVendor(vendor: VendorForCreation): Promise<Vendor> {
    return apiClient.post("/vendors", vendor);
  }

  public updateVendor(vendor: Vendor): Promise<void> {
    return apiClient.put("/vendors/" + vendor.id, vendor);
  }

  public deleteVendor(vendorId: string): Promise<void> {
    return apiClient.delete("/vendors/" + vendorId);
  }

  public getQueryString(vendorFilters: VendorFilters): string {
    const queryString = Object.keys(vendorFilters).map(key =>
      vendorFilters[key as keyof VendorFilters] !== undefined
        ? key + "=" + vendorFilters[key as keyof VendorFilters]
        : null
    );
    return queryString.filter(x => x !== null).join("&");
  }
}
