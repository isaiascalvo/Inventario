using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Util.Enums;

namespace Application.ViewModels
{
    public class CommissionFiltersViewModel
    {
        public Guid? VendorId { get; set; }
        public Guid? ClientId { get; set; }
        public string? ClientName { get; set; }
        public string? Product { get; set; }
        public decimal? Price { get; set; }
        public ePaymentTypes? PaymentType { get; set; }
        public string? DateFrom { get; set; }
        public string? DateTo { get; set; }
        public decimal? Value { get; set; }

        public bool IsEmpty()
        {
            return VendorId == null && ClientId == null && ClientName == null && Product == null && Price == null && PaymentType == null && DateFrom == null && DateTo == null && Value == null;
        }
    }
}
