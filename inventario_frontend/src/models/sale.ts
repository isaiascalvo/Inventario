import { Client } from "./client";
import { paymentTypes } from "@/enums/paymentTypes";
import { DetailForCreation, Detail } from "./detail";
import {
  OwnFeesForCreation,
  CashForCreation,
  CreditCardForCreation,
  DebitCardForCreation,
  ChequesPaymentForCreation,
  OwnFees,
  Cash,
  CreditCard,
  DebitCard,
  ChequesPayment
} from "./payment";

export class SaleForCreation {
  public clientId: string | undefined = undefined;
  public client: Client | undefined = undefined;
  public clientName: string | undefined = undefined;
  public date: Date | undefined = undefined;
  public paymentType: paymentTypes | undefined = undefined;
  public paymentId: string | undefined = undefined;
  public ownFees?: OwnFeesForCreation;
  public cash?: CashForCreation;
  public creditCard?: CreditCardForCreation;
  public debitCard?: DebitCardForCreation;
  public cheques?: ChequesPaymentForCreation;
  public details: DetailForCreation[] = [];
}

export class Sale extends SaleForCreation {
  public id!: string;
  public ownFees?: OwnFees;
  public cash?: Cash;
  public creditCard?: CreditCard;
  public debitCard?: DebitCard;
  public cheques?: ChequesPayment;
  public details: Detail[] = [];
}
