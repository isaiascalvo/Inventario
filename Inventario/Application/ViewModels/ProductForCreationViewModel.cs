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
        public string Code { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? VendorId { get; set; }
        public string Brand { get; set; }
        public double Stock { get; set; }
        public string UnitOfMeasurement { get; set; }
        public bool Active { get; set; }
        public bool Available { get; set; }
        public PriceForCreationViewModel Price { get; set; }
    }
}
