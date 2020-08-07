using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class PurchaseForCreationViewModel
    {
        public Guid ProductId { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public double Quantity { get; set; }
    }
}
