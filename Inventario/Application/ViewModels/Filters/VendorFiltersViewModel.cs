using Application.ViewModels;
using System;
using System.Linq.Expressions;

namespace Application.ViewModels
{
    public class VendorFiltersViewModel
    {
        public string? Name { get; set; }
        public string? CUIL { get; set; }
        public string? Phone { get; set; }
        public string? Mail { get; set; }
        public string? Description { get; set; }

        public bool IsEmpty()
        {
            return Name == null && CUIL == null && Phone == null && Mail == null && Description == null;
        }
    }
}