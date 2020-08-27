using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class FeeRuleForCreationViewModel
    {
        public Guid ProductId { get; set; }
        public int FeesAmountTo { get; set; }
        public double Percentage { get; set; }
    }
}
