using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class ChequesPayment: Payment
    {
        public List<Cheque> ListOfCheques { get; set; }


        public ChequesPayment()
        {
            ListOfCheques = new List<Cheque>();
        }
    }
}
