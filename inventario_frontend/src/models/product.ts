import { Category } from "./category";
import { Vendor } from "./vendor";
import { PriceForCreation, Price } from "./price";

export class ProductForCreation {
  public name: string | undefined = undefined;
  public description: string | undefined = undefined;
  public code: string | undefined = undefined;
  public brand: string | undefined = undefined;
  public categoryId: string | undefined = undefined;
  public category?: Category;
  public vendorId?: string;
  public vendor?: Vendor;
  public price: PriceForCreation;
  public stock = 0;
  public unitOfMeasurement: string | undefined = undefined;

  constructor() {
    this.price = new PriceForCreation();
    this.stock = 0;
  }
}

export class Product extends ProductForCreation {
  public id!: string;
  public price: Price;

  constructor() {
    super();
    this.price = new Price();
  }
}
