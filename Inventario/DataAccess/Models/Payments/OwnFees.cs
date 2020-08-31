using System;
using System.Collections.Generic;
using System.Text;
using Util.Enums;

namespace Data
{
    public class OwnFees: Payment
    {
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Guid? FeeRuleId { get; set; }
        public FeeRule? FeeRule { get; set; }
        public List<Fee> FeeList { get; set; }

        public OwnFees(DateTime expirationDate, double amount, int quantity, Guid createdBy)
        {
            ExpirationDate = expirationDate.ToLocalTime();
            Amount = amount;
            Quantity = quantity;
            FeeList = new List<Fee>();
            CreatedBy = createdBy;

            double feeValue = Math.Ceiling(Amount * 100 / Quantity) / 100;

            for (int i = 0; i < Quantity; i++)
            {
                if (i == Quantity - 1)
                {
                    var acum = feeValue * 100 * (Quantity - 1);
                    feeValue = (Amount * 100 - acum) / 100;

                }
                var fee = new Fee
                {
                    OwnFeesId = this.Id,
                    ExpirationDate = expirationDate.AddMonths(i).ToLocalTime(),
                    Value = feeValue, 
                    State = eFeeState.Pending,
                    CreatedBy = createdBy
                };

                FeeList.Add(fee);
            }
        }
    }
}
