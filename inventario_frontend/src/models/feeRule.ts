import { Product } from './product';

export class FeeRuleForCreation {
    public productId: string | undefined = undefined;
    public date: Date | undefined = undefined;
    public feesAmountTo: number | undefined = undefined;
    public percentage: number | undefined = undefined;
}

export class FeeRule extends FeeRuleForCreation {
    public id!: string;
    public product: Product | undefined = undefined;
}
