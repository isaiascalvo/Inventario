using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class FeeRuleViewModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public ProductViewModel Product { get; set; }
        public DateTime Date { get; set; }
        public int FeesAmountTo { get; set; }
        public double Percentage { get; set; }
    }
}
