using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class FeeRuleByCategoryViewModel
    {
        public Guid CategoryId { get; set; }
        public int FeesAmountTo { get; set; }
        public decimal Percentage { get; set; }
    }
}
