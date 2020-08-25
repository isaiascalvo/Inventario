export abstract class Payment {
  //Properties
  public amount: number | undefined = undefined;

  //Methods
  public abstract getPaymentType(): string;
  public getTotal(): number | void {
    return this.amount;
  }
}

export class CashForCreation extends Payment {
  public discount: number | undefined = undefined;

  public getPaymentType() {
    return "Efectivo";
  }
}

export class Cash extends CashForCreation {
  public id!: string;
}

export class ChequeForCreation extends Payment {
  public nro: string | undefined = undefined;
  public bank: string | undefined = undefined;

  public getPaymentType() {
    return "Cheque";
  }
}

export class Cheque extends ChequeForCreation {
  public id!: string;
}

export class DebitCardForCreation extends Payment {
  public cardType: string | undefined = undefined;
  public bank: string | undefined = undefined;

  public getPaymentType() {
    return "Tarjeta de débito";
  }
}

export class DebitCard extends DebitCardForCreation {
  public id!: string;
}

export class CreditCardForCreation extends Payment {
  public cardType: string | undefined = undefined;
  public bank: string | undefined = undefined;

  public getPaymentType() {
    return "Tarjeta de crédito";
  }
}

export class CreditCard extends CreditCardForCreation {
  public id!: string;
}

export class OwnFeesForCreation extends Payment {
  public quantity = 1;
  public expirationDate: Date | undefined = undefined;
  public feeList: Array<Fee>;

  /**
   *
   */
  constructor() {
    super();
    this.feeList = new Array<Fee>();
  }

  public getTotal(): number | void {
    if (this.quantity && this.amount) {
      return this.quantity * this.amount;
    }
  }

  public getPaymentType() {
    return "Cuotas Propias";
  }
}

export class OwnFees extends OwnFeesForCreation {
  public id!: string;
}

export class Fee {
  public id!: string;
  public ownFeesId!: string;
  public expirationDate!: Date;
  public paymentDate?: Date;
}
