using Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Logic.Dtos
{
    public class ProductDto: Entity
    {
        //public Guid Id { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime? LastModificationAt { get; set; }
        //public bool IsDeleted { get; set; }
        //public DateTime? DeletedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryDto Category { get; set; }
        public Guid VendorId { get; set; }
        public VendorDto Vendor { get; set; }
        public string Brand { get; set; }
        public double Stock { get; set; }
        public string UnitOfMeasurement { get; set; }
        public PriceDto Price { get; set; }
    }
}
