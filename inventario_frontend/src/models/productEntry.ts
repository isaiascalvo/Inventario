import { paymentTypes } from "@/enums/paymentTypes";
import { ProductEntryLine } from "./productEntryLine";
import { Vendor } from "./vendor";

export class ProductEntryForCreation {
  public date: Date | undefined = undefined;
  public vendorId?: string;
  public cost?: number;
  public paymentType?: paymentTypes;
  public observations?: string;
  public isEntry = true;
  public productEntryLines: ProductEntryLine[] = [];
}

export class ProductEntry extends ProductEntryForCreation {
  public id!: string;
  public vendor?: Vendor;
}
