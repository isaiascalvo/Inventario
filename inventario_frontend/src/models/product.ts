import { Category } from "./category";
import { Vendor } from "./vendor";
import { Price } from './price';

export class ProductForCreation {
  public name!: string;
  public description!: string;
  public code!: string;
  public brand!: string;
  public active!: boolean;
  public available!: boolean;
  public categoryId!: string;
  public category?: Category;
  public vendorId?: string;
  public vendor?: Vendor;
  public price?: Price;

  constructor() {
    this.name = "";
    this.description = "";
    this.code = "";
    this.brand = "";
    this.active = true;
    this.available = true;
    this.categoryId = "";
    this.vendorId = "";
    this.price = new Price();
  }
}

export class Product extends ProductForCreation {
  public id!: string;
}

