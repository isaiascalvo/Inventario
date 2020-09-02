import { paymentTypes } from "@/enums/paymentTypes";

export class SaleFilters {
  public clientName?: string;
  public clientId?: string;
  public productId?: string;
  public paymentType?: paymentTypes;
  public dateDateFrom?: Date;
  public dateFrom?: string;
  public dateDateTo?: Date;
  public dateTo?: string;
}
