using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class FeeRuleForCreationDto
    {
        public Guid ProductId { get; set; }
        public int FeesAmountTo { get; set; }
        public decimal Percentage { get; set; }
    }
}
