using Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Logic.Dtos
{
    public class ProductFiltersDto
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? VendorId { get; set; }
        public string? Brand { get; set; }
        public DateTime? Date { get; set; }

        public bool IsEmpty()
        {
            return Code == null && Name == null && Description == null && CategoryId == null && VendorId == null && Brand == null && Date == null;
        }

        public Expression<Func<Product, bool>> GetExpresion()
        {
            return x =>
                (Code == null || x.Code == Code) &&
                (Name == null || x.Name.ToLower().Contains(Name.ToLower())) &&
                (Description == null || x.Description.ToLower().Contains(Description.ToLower())) &&
                (CategoryId == null || x.CategoryId == CategoryId) &&
                (VendorId == null || x.VendorId == VendorId) &&
                (Brand == null || x.Brand.ToLower().Contains(Brand.ToLower()) &&
                (Date == null || x.CreatedAt <= Date));
        }
    }
}
