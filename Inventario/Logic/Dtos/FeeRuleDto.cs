using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class FeeRuleDto: Entity
    {
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
        public DateTime Date { get; set; }
        public int FeesAmountTo { get; set; }
        public decimal Percentage { get; set; }
    }
}
