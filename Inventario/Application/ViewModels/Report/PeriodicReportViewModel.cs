using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class PeriodicReportViewModel
    {
        public string Period { get; set; }
        public decimal FixedCosts { get; set; }
        public decimal VariableCosts { get; set; }
        public decimal Sales { get; set; }
        public decimal Purchases { get; set; }
        public decimal Commissions { get; set; }
    }
}
