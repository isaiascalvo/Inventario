export class MiscellaneousExpensesForCreation {
  public description: string | undefined = undefined;
  public date: Date | undefined = undefined;
  public value: number | undefined = undefined;
  public destination?: string | undefined = undefined;
  public isFixed: boolean | undefined = undefined;
}

export class MiscellaneousExpenses extends MiscellaneousExpensesForCreation {
  public id!: string;
}
