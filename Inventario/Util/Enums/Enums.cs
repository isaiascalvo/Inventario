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
        Cheques
    }

    public enum eFeeState
    {
        Pending,
        Paid
    }

    public enum ePriceTypes
    {
        PurchasePrice,
        SalePrice
    }
    public enum eMonths
    {
        Enero,
        Febrero,
        Marzo,
        Abril,
        Mayo,
        Junio,
        Julio,
        Agosto,
        Septiembre,
        Octubre,
        Noviembre,
        Diciembre
    }

    public static class eRoutes
    {
        public const string
            Html = "/html";
    }

}
