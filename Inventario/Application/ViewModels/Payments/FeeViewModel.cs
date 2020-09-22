using Data;
using System;
using System.Collections.Generic;
using System.Text;
using Util.Enums;

namespace Application.ViewModels
{
    public class FeeViewModel: Entity
    {
        public Guid OwnFeesId { get; set; }
        public OwnFeesViewModel OwnFees { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal Value { get; set; }
    }
}
