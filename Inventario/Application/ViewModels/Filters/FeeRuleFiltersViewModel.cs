using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class FeeRuleFiltersViewModel
    {
        public Guid? ProductId { get; set; }
        public int? FeesAmountTo { get; set; }
        public decimal? Percentage { get; set; }

        public bool IsEmpty()
        {
            return ProductId == null && FeesAmountTo == null && Percentage == null;
        }
    }
}
