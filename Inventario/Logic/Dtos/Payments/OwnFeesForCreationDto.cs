﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class OwnFeesForCreationDto : PaymentForCreationDto
    {
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        //public List<FeeForCreationDto> FeeList;

        //public OwnFeeesDto()
        //{
        //    FeeList = new List<FeeDto>();

        //    for (int i = 0; i < Quantity; i++)
        //    {
        //        var fee = new FeeDto
        //        {
        //            OwnFeesId = this.Id,
        //            ExpirationDate = ExpirationDate.AddMonths(i)
        //        };

        //        FeeList.Add(fee);
        //    }
        //}
    }
}
