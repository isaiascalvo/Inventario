using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModels
{
    public class OwnFeesForCreationViewModel : PaymentForCreationViewModel
    {
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Guid? FeeRuleId { get; set; }
        public List<FeeForCreationViewModel> FeeList;

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
