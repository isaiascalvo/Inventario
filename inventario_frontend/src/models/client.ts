export class ClientForCreation {
  public name: string | undefined = undefined;
  public lastname: string | undefined = undefined;
  public dni: string | undefined = undefined;
  public phone: string | undefined = undefined;
  public mail?: string;
  public birthdate: Date | undefined = undefined;
}

export class Client extends ClientForCreation {
  public id!: string;
  public debtor!: boolean;
}
