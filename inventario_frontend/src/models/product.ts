import { Category } from "./category";
import { Vendor } from "./vendor";
import { PriceForCreation, Price } from "./price";

export class ProductForCreation {
  public name: string | undefined = undefined;
  public description: string | undefined = undefined;
  public code?: string;
  public brand: string | undefined = undefined;
  public categoryId: string | undefined = undefined;
  public category?: Category;
  public vendorId?: string;
  public vendor?: Vendor;
  public purchasePrice: PriceForCreation;
  public salePrice: PriceForCreation;
  public minimumStock?: number;
  public stock = 0;
  public unitOfMeasurement: string | undefined = undefined;

  constructor() {
    this.purchasePrice = new PriceForCreation();
    this.salePrice = new PriceForCreation();
    this.stock = 0;
  }
}

export class Product extends ProductForCreation {
  public id!: string;
  public purchasePrice: Price;
  public salePrice: Price;

  constructor() {
    super();
    this.purchasePrice = new Price();
    this.salePrice = new Price();
  }
}
