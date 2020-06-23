import { Category } from "./category";
// import { Unit } from "./unit";
// import { Currency } from "./currency";
// import { Customer } from "./customer";
// import { Activity } from "./activity";
// import { Partner } from "./partner";

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
  public price!: number;

  // public sourceLanguage?: Language;
  // public targetLanguage?: Language;
  // public unit?: Unit;
  // public currency?: Currency;
  // public activity?: Activity;
  // public partner?: Partner;
  // public customer?: Customer;
}

export class Product extends ProductForCreation {
  public id!: string;
}
