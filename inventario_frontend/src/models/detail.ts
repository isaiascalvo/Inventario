import { Product } from "./product";

export class DetailForCreation {
  public productId: string | undefined = undefined;
  public quantity: number | undefined = undefined;
}

export class Detail extends DetailForCreation {
  public id!: string;
  public saleId!: string;
  public product: Product | undefined = undefined;
  public unitPrice!: number;
}
