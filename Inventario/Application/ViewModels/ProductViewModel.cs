using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Util.Enums;

namespace Application.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime? LastModificationAt { get; set; }
        //public bool IsDeleted { get; set; }
        //public DateTime? DeletedAt { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public string? Code { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryViewModel Category { get; set; }
        public Guid? VendorId { get; set; }
        public VendorViewModel Vendor { get; set; }
        public string Brand { get; set; }
        public decimal? MinimumStock { get; set; }
        public decimal Stock { get; set; }
        public string UnitOfMeasurement { get; set; }
        public PriceViewModel PurchasePrice { get; set; }
        public PriceViewModel SalePrice { get; set; }
    }
}
