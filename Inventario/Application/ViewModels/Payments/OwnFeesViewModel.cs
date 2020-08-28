using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModels
{
    public class OwnFeesViewModel: PaymentViewModel
    {
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Guid? FeeRuleId { get; set; }
        public FeeRuleViewModel? FeeRule { get; set; }
        public List<FeeViewModel> FeeList;

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
