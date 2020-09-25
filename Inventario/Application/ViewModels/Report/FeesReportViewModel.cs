using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class FeesReportViewModel
    {
        public DateTime Date { get; set; }
        public int FeesQty { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Capital { get; set; }
        public decimal Interest { get; set; }
        public decimal FeeValue { get; set; }
    }
}
