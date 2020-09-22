using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Util.Enums;

namespace Application.ViewModels
{
    public class CommissionForCreationViewModel
    {
        public Guid VendorId { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
        public ePaymentTypes PaymentType { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }
}
