﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModels
{
    public class CreditCardForCreationViewModel : PaymentForCreationViewModel
    {
        public string CardType { get; set; }
        public string Bank { get; set; }
    }
}
