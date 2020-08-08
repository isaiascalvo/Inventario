export class VendorForCreation {
  public name: string | undefined = undefined;
  public cuil: string | undefined = undefined;
  public phone: string | undefined = undefined;
  public mail: string | undefined = undefined;
  public active = true;
  public description: string | undefined = undefined;
}

export class Vendor extends VendorForCreation {
  public id!: string;
}
