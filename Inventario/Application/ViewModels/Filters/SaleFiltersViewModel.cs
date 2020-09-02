using Application.ViewModels;
using System;
using System.Linq.Expressions;
using Util.Enums;

namespace Application.ViewModels
{
    public class SaleFiltersViewModel
    {
        public string? ClientName { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? ProductId { get; set; }
        public string? DateFrom { get; set; }
        public string? DateTo { get; set; }
        public ePaymentTypes? PaymentType { get; set; }

        public bool IsEmpty()
        {
            return ClientName == null && ClientId == null && ProductId == null && DateFrom == null && DateTo == null && PaymentType == null;
        }
    }
}