import { ProductEntryLine } from "./productEntryLine";

export class ProductEntryForCreation {
  public date: Date | undefined = undefined;
  public isEntry = true;
  public productEntryLines: ProductEntryLine[] = [];
}

export class ProductEntry extends ProductEntryForCreation {
  public id!: string;
}
