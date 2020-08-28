using Data;
using System;
using System.Collections.Generic;
using System.Text;
using Util.Enums;

namespace Logic.Dtos
{
    public class FeeDto: Entity
    {
        public Guid OwnFeesId { get; set; }
        public OwnFeesDto OwnFees { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public double Value { get; set; }
        public eFeeState State { get; set; }
    }
}
