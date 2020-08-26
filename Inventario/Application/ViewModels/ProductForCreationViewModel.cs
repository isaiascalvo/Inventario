using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Util.Enums;

namespace Application.ViewModels
{
    public class ProductForCreationViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Code { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? VendorId { get; set; }
        public string Brand { get; set; }
        public double? MinimumStock { get; set; }
        public double Stock { get; set; }
        public string UnitOfMeasurement { get; set; }
        public PriceForCreationViewModel PurchasePrice { get; set; }
        public PriceForCreationViewModel SalePrice { get; set; }
    }
}
