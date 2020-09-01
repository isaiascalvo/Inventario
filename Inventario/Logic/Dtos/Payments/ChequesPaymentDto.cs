using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class ChequesPaymentDto: PaymentDto
    {
        public List<ChequeDto> ListOfCheques { get; set; }
    }
}
