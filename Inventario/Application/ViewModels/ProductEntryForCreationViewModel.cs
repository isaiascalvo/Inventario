using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Util.Enums;

namespace Application.ViewModels
{
    public class ProductEntryForCreationViewModel
    {
        public DateTime Date { get; set; }
        public bool IsEntry { get; set; }
        public Guid? VendorId { get; set; }
        public decimal? Cost { get; set; }
        public ePaymentTypes? PaymentType { get; set; }
        public string Observations { get; set; }
        public List<ProductEntryLineForCreationViewModel> ProductEntryLines { get; set; }
    }
}
