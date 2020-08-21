export class VendorForCreation {
  public name: string | undefined = undefined;
  public cuil?: string;
  public phone: string | undefined = undefined;
  public mail?: string;
  // public active = true;
  public description?: string;
}

export class Vendor extends VendorForCreation {
  public id!: string;
}
