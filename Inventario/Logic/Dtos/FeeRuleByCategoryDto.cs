using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class FeeRuleByCategoryDto
    {
        public Guid CategoryId { get; set; }
        public int FeesAmountTo { get; set; }
        public decimal Percentage { get; set; }
    }
}
