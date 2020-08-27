﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Fee: Entity
    {
        public Guid OwnFeesId { get; set; }
        public OwnFees OwnFees { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}