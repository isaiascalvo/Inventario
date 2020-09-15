import { Vendor } from "./vendor";
import { Client } from "./client";
import { paymentTypes } from "@/enums/paymentTypes";

export class CommissionForCreation {
  public vendorId: string | undefined = undefined;
  public vendor: Vendor | undefined = undefined;
  public clientId?: string;
  public client?: Client;
  public clientName: string | undefined = undefined;
  public product: string | undefined = undefined;
  public price: number | undefined = undefined;
  public paymentType: paymentTypes | undefined = undefined;
  public date: Date | undefined = undefined;
  public value: number | undefined = undefined;
}

export class Commission extends CommissionForCreation {
  public id!: string;
}
