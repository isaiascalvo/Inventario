export abstract class Payment {
  //Properties
  public amount: number | undefined = undefined;

  //Methods
  public getTotal(): number | void {
    return this.amount;
  }
}

export class CashForCreation extends Payment {
  public discount: number | undefined = undefined;

  public static GetPaymentType() {
    return "Efectivo";
  }
}

export class Cash extends CashForCreation {
  public id!: string;
}

export class ChequesPaymentForCreation extends Payment {
  public listOfCheques: ChequeForCreation[] = [];

  public static GetPaymentType() {
    return "Cheque/s";
  }
}

export class ChequesPayment extends ChequesPaymentForCreation {
  public id!: string;
  public listOfCheques: Cheque[] = [];
}

export class ChequeForCreation {
  public nro: string | undefined = undefined;
  public bank: string | undefined = undefined;
  public value: number | undefined = undefined;
}

export class Cheque extends ChequeForCreation {
  public id!: string;
  public chequesPaymentId!: string;
}

export class DebitCardForCreation extends Payment {
  public cardType: string | undefined = undefined;
  public bank: string | undefined = undefined;
  public discount: number | undefined = undefined;
  public surcharge: number | undefined = undefined;

  public static GetPaymentType() {
    return "Tarjeta de débito";
  }
}

export class DebitCard extends DebitCardForCreation {
  public id!: string;
}

export class CreditCardForCreation extends Payment {
  public cardType: string | undefined = undefined;
  public bank: string | undefined = undefined;
  public discount: number | undefined = undefined;
  public surcharge: number | undefined = undefined;

  public static GetPaymentType() {
    return "Tarjeta de crédito";
  }
}

export class CreditCard extends CreditCardForCreation {
  public id!: string;
}

export class OwnFeesForCreation extends Payment {
  public quantity: number | undefined = undefined;
  public expirationDate: Date | undefined = undefined;

  /**
   *
   */
  constructor() {
    super();
  }

  public getTotal(): number | void {
    if (this.quantity && this.amount) {
      return this.quantity * this.amount;
    }
  }

  public static GetPaymentType() {
    return "Cuotas Propias";
  }
}

export class OwnFees extends OwnFeesForCreation {
  public id!: string;
  public feeList: Array<Fee>;
  /**
   *
   */
  constructor() {
    super();
    this.feeList = new Array<Fee>();
  }
}

export class Fee {
  public id!: string;
  public ownFeesId!: string;
  public expirationDate!: Date;
  public paymentDate?: Date;
  public value!: number;
}
