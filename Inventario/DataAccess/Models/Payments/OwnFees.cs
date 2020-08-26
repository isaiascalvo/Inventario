using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class OwnFees: Payment
    {
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public List<Fee> FeeList;

        public OwnFees(DateTime expirationDate, Guid createdBy)
        {
            ExpirationDate = expirationDate.ToLocalTime();
            FeeList = new List<Fee>();

            for (int i = 0; i < Quantity; i++)
            {
                var fee = new Fee
                {
                    OwnFeesId = this.Id,
                    ExpirationDate = expirationDate.AddMonths(i).ToLocalTime(),
                    CreatedBy = createdBy
                };

                FeeList.Add(fee);
            }
        }
    }
}
