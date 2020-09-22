using Data;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Util.Enums;

namespace Logic.Dtos
{
    public class CommissionFiltersDto
    {
        public Guid? VendorId { get; set; }
        public Guid? ClientId { get; set; }
        public string? ClientName { get; set; }
        public string? Product { get; set; }
        public decimal? Price { get; set; }
        public ePaymentTypes? PaymentType { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public decimal? Value { get; set; }

        public bool IsEmpty()
        {
            return VendorId == null && ClientId == null && ClientName == null && Product == null && Price == null && PaymentType == null && DateFrom== null && DateTo== null && Value == null;
        }

        public Expression<Func<Commission, bool>> GetExpresion()
        {
            return x =>
                (VendorId == null || x.VendorId == VendorId) &&
                (ClientId == null || x.ClientId != null && x.ClientId == ClientId) &&
                (ClientName == null || x.ClientName.ToLower().Contains(ClientName.ToLower())) &&
                (Product == null || x.Product.ToLower().Contains(Product.ToLower())) &&
                (Price == null || x.Price == Price) &&
                (PaymentType == null || x.PaymentType == PaymentType) &&
                (DateFrom == null || x.Date >= DateFrom) &&
                (DateTo == null || x.Date <= DateTo) &&
                (Value == null || x.Value == Value);
        }
    }
}
