import { Product } from "./product";
import { Client } from "./client";

export class PurchaseForCreation {
  public productId: string | undefined = undefined;
  public product: Product | undefined = undefined;
  public clientId: string | undefined = undefined;
  public client: Client | undefined = undefined;
  public clientName: string | undefined = undefined;
  public date: Date | undefined = undefined;
  public quantity: number | undefined = undefined;
  public amount: number | undefined = undefined;
}

export class Purchase extends PurchaseForCreation {
  public id!: string;
}
