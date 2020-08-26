using System;
using System.Collections.Generic;
using System.Text;

namespace Util.Enums
{
    public enum ePaymentTypes
    {
        Cash,
        OwnFees,
        CreditCard,
        DebitCard,
        Cheque
    }

    public enum ePriceTypes
    {
        PurchasePrice,
        SalePrice
    }

    public static class eRoutes
    {
        public const string
            Html = "/html";
    }

}
