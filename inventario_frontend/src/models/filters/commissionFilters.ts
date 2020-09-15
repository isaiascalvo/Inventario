import { paymentTypes } from "@/enums/paymentTypes";

export class CommissionFilters {
  public vendorId?: string;
  public clientName?: string;
  public clientId?: string;
  public product?: string;
  public price?: number;
  public paymentType?: paymentTypes;
  public dateDateFrom?: Date;
  public dateFrom?: string;
  public dateDateTo?: Date;
  public dateTo?: string;
  public value?: number;
}
