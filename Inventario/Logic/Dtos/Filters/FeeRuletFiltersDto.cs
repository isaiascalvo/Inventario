using Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Logic.Dtos
{
    public class FeeRuleFiltersDto
    {
        public Guid? ProductId { get; set; }
        public int? FeesAmountTo { get; set; }
        public double? Percentage { get; set; }

        public bool IsEmpty()
        {
            return ProductId == null && FeesAmountTo == null && Percentage == null;
        }

        public Expression<Func<FeeRule, bool>> GetExpresion()
        {
            return x =>
                (ProductId == null || x.ProductId == ProductId) &&
                (FeesAmountTo == null || x.FeesAmountTo == FeesAmountTo) &&
                (Percentage == null || x.Percentage == Percentage);
        }
    }
}
