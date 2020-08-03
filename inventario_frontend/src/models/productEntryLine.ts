export class ProductEntryLineForCreation {
  public quantity: number | undefined = undefined;
  public productId: string | undefined = undefined;
  public poductEntryId: string | undefined = undefined;
}

export class ProductEntryLine extends ProductEntryLineForCreation {
  public id: string | undefined = undefined;
}
