using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class CommissionForCreationViewModel
    {
        public Guid VendorId { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }
        public string Product { get; set; }
        public double Price { get; set; }
        public string PaymentType { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
}
