using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Util.Enums;

namespace Data
{
    public class Product: Entity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string Code { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid VendorId { get; set; }
        public Vendor Vendor { get; set; }
        public string Brand { get; set; }
        public double Stock { get; set; }
        public string UnitOfMeasurement { get; set; }
    }
}
