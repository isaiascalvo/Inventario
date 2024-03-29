﻿using Application.ViewModels;
using System;
using System.Linq.Expressions;

namespace Application.ViewModels
{
    public class ProductFiltersViewModel
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? VendorId { get; set; }
        public string? Brand { get; set; }
        public string? Date { get; set; }

        public bool IsEmpty()
        {
            return Code == null && Name == null && Description == null && CategoryId == null && VendorId == null && Brand == null && Date == null;
        }
    }
}