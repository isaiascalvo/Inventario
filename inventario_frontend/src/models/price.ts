export class PriceForCreation {
    public value = 0;
    public datetime: Date | undefined = undefined;
    public productId: string | undefined = undefined;
}
  
export class Price extends PriceForCreation {
    public id!: string;
}