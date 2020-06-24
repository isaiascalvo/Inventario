export class PriceForCreation {
    public value!: number;
    public datetime!: Date;
    public productId!: string;
}
  
export class Price extends PriceForCreation {
    public id!: string;
}