using Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Logic.Dtos
{
    public class MiscellaneousExpensesFiltersDto
    {
        public string? Description { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public double? Value { get; set; }
        public string? Destination { get; set; }
        public bool? IsFixed { get; set; }

        public bool IsEmpty()
        {
            return Description == null && DateFrom == null && DateTo == null && Value == null && Destination == null && IsFixed == null;
        }

        public Expression<Func<MiscellaneousExpenses, bool>> GetExpresion()
        {
            return x =>
                (Description == null || x.Description.ToLower().Contains(Description.ToLower())) &&
                (DateFrom == null || x.Date >= DateFrom) &&
                (DateTo == null || x.Date <= DateTo) &&
                (Value == null || x.Value == Value) &&
                (Destination == null || x.Destination != null && x.Destination.ToLower().Contains(Destination.ToLower())) &&
                (IsFixed == null || x.IsFixed == IsFixed);
        }
    }
}
