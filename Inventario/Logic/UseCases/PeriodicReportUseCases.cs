using AutoMapper;
using Infrastructure.Repositories;
using Logic.Dtos;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Enums;

namespace Logic
{
    public class PeriodicReportUseCases: IPeriodicReportUseCases
    {
        private readonly ISaleRepository _saleRepositoryRepository;
        private readonly IProductEntryRepository _productEntryRepository;
        private readonly ICommissionRepository _commissionRepository;
        private readonly IMiscellaneousExpensesRepository _miscellaneousExpensesRepository;
        private readonly IMapper _mapper;

        public PeriodicReportUseCases(ISaleRepository saleRepositoryRepository, IProductEntryRepository productEntryRepository, ICommissionRepository commissionRepository, IMiscellaneousExpensesRepository miscellaneousExpensesRepository, IMapper mapper)
        {
            _saleRepositoryRepository = saleRepositoryRepository;
            _productEntryRepository = productEntryRepository;
            _commissionRepository = commissionRepository;
            _miscellaneousExpensesRepository = miscellaneousExpensesRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PeriodicReportDto>> GetByYearAndMonth(int year)
        {
            List<PeriodicReportDto> periodicReports = new List<PeriodicReportDto>();

            for (int i = 1; i <= 12; i++)
            {
                PeriodicReportDto periodicReport = new PeriodicReportDto();
                periodicReport.Period = Enum.GetName(typeof(eMonths), i - 1);
                    
                periodicReport.Sales = (
                        await _saleRepositoryRepository.Find(x => x.Date.Year == year && x.Date.Month == i, x => x.Include(s => s.Payment))
                    ).Sum(x => x.Payment.Amount);

                periodicReport.Purchases = (
                        await _productEntryRepository.Find(x => x.Date.Year == year && x.Date.Month == i && x.IsEntry)
                    ).Sum(x => x.Cost.HasValue ? x.Cost.Value : 0);

                periodicReport.Commissions = (
                        await _commissionRepository.Find(x => x.Date.Year == year && x.Date.Month == i)
                    ).Sum(x => x.Value);

                periodicReport.FixedCosts = (
                        await _miscellaneousExpensesRepository.Find(x => x.Date.Year == year && x.Date.Month == i && x.IsFixed)
                    ).Sum(x => x.Value);

                periodicReport.VariableCosts = (
                        await _miscellaneousExpensesRepository.Find(x => x.Date.Year == year && x.Date.Month == i && !x.IsFixed)
                    ).Sum(x => x.Value);

                periodicReports.Add(periodicReport);
            }

            return periodicReports;
        }
    }
}
