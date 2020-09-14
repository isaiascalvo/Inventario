using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class CommissionFiltersViewModel
    {
        public Guid? VendorId { get; set; }
        public Guid? ClientId { get; set; }
        public string? ClientName { get; set; }
        public string? Product { get; set; }
        public double? Price { get; set; }
        public string? PaymentType { get; set; }
        public string? DateFrom { get; set; }
        public string? DateTo { get; set; }
        public double? Value { get; set; }

        public bool IsEmpty()
        {
            return VendorId == null && ClientId == null && ClientName == null && Product == null && Price == null && PaymentType == null && DateFrom == null && DateTo == null && Value == null;
        }
    }
}
