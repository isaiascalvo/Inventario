using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class ChequesPaymentForCreationDto: PaymentForCreationDto
    {
        public List<ChequeForCreationDto> ListOfCheques { get; set; }
    }
}
