export class VendorForCreation {
    public name!: string;
    public cuil!: string;
    public phone!: string;
    public mail!: string;
    public active!: boolean;
    public description!: string;

    constructor() {
       this.name = "";
       this.cuil = "";
       this.phone = "";
       this.mail = "";
       this.active = true;
       this.description = "";
    }
}
  
export class Vendor extends VendorForCreation {
    public id!: string;
}