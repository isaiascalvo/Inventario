import { FeeStates } from "@/enums/paymentTypes";
import { FeeRule } from "./feeRule";

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

export class ChequeForCreation extends Payment {
  public nro: string | undefined = undefined;
  public bank: string | undefined = undefined;

  public static GetPaymentType() {
    return "Cheque";
  }
}

export class Cheque extends ChequeForCreation {
  public id!: string;
}

export class DebitCardForCreation extends Payment {
  public cardType: string | undefined = undefined;
  public bank: string | undefined = undefined;
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

  public static GetPaymentType() {
    return "Tarjeta de crédito";
  }
}

export class CreditCard extends CreditCardForCreation {
  public id!: string;
}

export class OwnFeesForCreation extends Payment {
  public quantity = 1;
  public expirationDate: Date | undefined = undefined;
  public feeRuleId?: string;
  public feeRule?: FeeRule;

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
  public state!: FeeStates;
  public value!: number;
}
