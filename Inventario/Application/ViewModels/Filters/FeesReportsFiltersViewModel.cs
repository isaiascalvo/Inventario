using Application.ViewModels;
using System;
using System.Linq.Expressions;
using Util.Enums;

namespace Application.ViewModels
{
    public class FeesReportsFiltersViewModel
    { 
        public string? DateFrom { get; set; }
        public string? DateTo { get; set; }

        public bool IsEmpty()
        {
            return DateFrom == null && DateTo == null;
        }
    }
}