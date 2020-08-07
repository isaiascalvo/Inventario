using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Purchase: Entity
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid? ClientId { get; set; }
        public Client Client { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public double Quantity { get; set; }
        public double Amount { get; set; }
    }
}
