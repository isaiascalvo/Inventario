import { apiClient } from "./apiClient";
import { Vendor, VendorForCreation } from "@/models/vendor";

export interface VendorService {
  getVendors(): Promise<Vendor[]>;
  getVendor(id: string): Promise<Vendor>;
  addVendor(vendor: Vendor): Promise<Vendor>;
  updateVendor(vendor: Vendor): Promise<void>;
  deleteVendor(vendorId: string): Promise<void>;
}

export class NavigatorVendorService implements VendorService {
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
}
