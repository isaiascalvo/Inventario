﻿using Data;
using System;
using System.Collections.Generic;
using System.Text;
using Util.Enums;

namespace Logic.Dtos
{
    public class CommissionDto : Entity
    {
        public Guid VendorId { get; set; }
        public VendorDto Vendor { get; set; }
        public Guid? ClientId { get; set; }
        public ClientDto Client { get; set; }
        public string ClientName { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
        public ePaymentTypes PaymentType { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }
}
