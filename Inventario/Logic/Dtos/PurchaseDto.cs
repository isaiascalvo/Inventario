using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class PurchaseDto: Entity
    {
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
        public Guid? ClientId { get; set; }
        public ClientDto Client { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public double Quantity { get; set; }
        public double Amount { get; set; }

    }
}
