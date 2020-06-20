using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class PriceDto: Entity
    {
        //public Guid Id { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime? LastModificationAt { get; set; }
        //public bool IsDeleted { get; set; }
        //public DateTime? DeletedAt { get; set; }
        public double Value { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
        public DateTime DateTime { get; set; }
    }
}
