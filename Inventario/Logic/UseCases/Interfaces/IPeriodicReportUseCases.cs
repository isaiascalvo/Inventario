using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IPeriodicReportUseCases
    {
        Task<IEnumerable<PeriodicReportDto>> GetAnnualReport(int year);
        Task<IEnumerable<FeesReportDto>> GetFeesReport(FeesReportsFiltersDto filtersDto);
    }
}
