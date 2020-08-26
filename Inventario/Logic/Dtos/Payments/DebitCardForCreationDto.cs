using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class DebitCardForCreationDto : PaymentForCreationDto
    {
        public string CardType { get; set; }
        public string Bank { get; set; }
    }
}
