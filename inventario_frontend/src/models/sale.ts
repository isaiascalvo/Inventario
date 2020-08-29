import { Product } from "./product";
import { Client } from "./client";
import { paymentTypes } from "@/enums/paymentTypes";
import {
  OwnFees,
  Cash,
  CreditCard,
  DebitCard,
  Cheque,
  OwnFeesForCreation,
  CashForCreation,
  CreditCardForCreation,
  DebitCardForCreation,
  ChequeForCreation
} from "./payment";

export class SaleForCreation {
  public productId: string | undefined = undefined;
  public product: Product | undefined = undefined;
  public clientId: string | undefined = undefined;
  public client: Client | undefined = undefined;
  public clientName: string | undefined = undefined;
  public date: Date | undefined = undefined;
  public quantity: number | undefined = undefined;
  public amount: number | undefined = undefined;
  public paymentType: paymentTypes | undefined = undefined;
  public paymentId: string | undefined = undefined;
  public ownFees?: OwnFeesForCreation;
  public cash?: CashForCreation;
  public creditCard?: CreditCardForCreation;
  public debitCard?: DebitCardForCreation;
  public cheque?: ChequeForCreation;
}

export class Sale extends SaleForCreation {
  public id!: string;
  public ownFees?: OwnFees;
  public cash?: Cash;
  public creditCard?: CreditCard;
  public debitCard?: DebitCard;
  public cheque?: Cheque;
}
