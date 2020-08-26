using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class Cheque: Payment
    {
        public string Nro { get; set; }
        public string Bank { get; set; }
    }
}
