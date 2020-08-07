﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class PurchaseForCreationDto
    {
        public Guid ProductId { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public double Quantity { get; set; }
    }
}