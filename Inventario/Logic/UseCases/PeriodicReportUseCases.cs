using AutoMapper;
using Data;
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
        private readonly IOwnFeesRepository _ownFeesRepository;
        private readonly IFeeRuleRepository _feeRuleRepository;
        private readonly IMapper _mapper;

        public PeriodicReportUseCases(ISaleRepository saleRepositoryRepository, IProductEntryRepository productEntryRepository, ICommissionRepository commissionRepository, IMiscellaneousExpensesRepository miscellaneousExpensesRepository, IOwnFeesRepository ownFeesRepository, IFeeRuleRepository feeRuleRepository, IMapper mapper)
        {
            _saleRepositoryRepository = saleRepositoryRepository;
            _productEntryRepository = productEntryRepository;
            _commissionRepository = commissionRepository;
            _miscellaneousExpensesRepository = miscellaneousExpensesRepository;
            _ownFeesRepository = ownFeesRepository;
            _feeRuleRepository = feeRuleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PeriodicReportDto>> GetAnnualReport(int year)
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

            PeriodicReportDto finalResultReport = new PeriodicReportDto();
            finalResultReport.Period = "Totales";
            finalResultReport.Sales = periodicReports.Sum(x => x.Sales);
            finalResultReport.Purchases = periodicReports.Sum(x => x.Purchases);
            finalResultReport.Commissions = periodicReports.Sum(x => x.Commissions);
            finalResultReport.FixedCosts = periodicReports.Sum(x => x.FixedCosts);
            finalResultReport.VariableCosts = periodicReports.Sum(x => x.VariableCosts);

            periodicReports.Add(finalResultReport);
            return periodicReports;
        }

        public async Task<IEnumerable<FeesReportDto>> GetFeesReport(FeesReportsFiltersDto filtersDto)
        {
            List<FeesReportDto> feesReports = new List<FeesReportDto>();

            var sales = (await _saleRepositoryRepository
                            .Find(x => x.PaymentType == ePaymentTypes.OwnFees, x => x.Include(s => s.Details))
                        ).AsQueryable().Where(filtersDto.GetExpresion()).OrderBy(x => x.Date);

            foreach (var sale in sales)
            {
                FeesReportDto feesReport = new FeesReportDto();
                feesReport.Date = sale.Date;
                feesReport.ClientId = sale.ClientId;
                feesReport.ClientName = sale.ClientName;

                var ownFee = (await _ownFeesRepository.Find(x => x.SaleId == sale.Id, x => x.Include(s => s.FeeList))).FirstOrDefault();
                feesReport.FeesQty = ownFee.Quantity;
                feesReport.TotalAmount = ownFee.Amount;
                feesReport.FeeValue = ownFee.FeeList.OrderBy(x => x.ExpirationDate).FirstOrDefault().Value;

                decimal percentage = 0;
                foreach (var detail in sale.Details)
                {
                    if (!detail.FeeRuleId.HasValue)
                        throw new InvalidOperationException("FeeRuleId is empty");
                
                    var rule = await _feeRuleRepository.GetById(detail.FeeRuleId.Value);
                    if (rule == null)
                        rule = (await _feeRuleRepository.FindDeleted(x => x.Id == detail.FeeRuleId.Value)).FirstOrDefault();
                    if (rule == null)
                        throw new KeyNotFoundException($"Fee Rule with id: {detail.FeeRuleId.Value} not found.");

                    percentage += rule.Percentage;
                }

                feesReport.Interest = feesReport.FeeValue * (percentage * ownFee.Quantity / 100);
                feesReport.Capital = feesReport.FeeValue - feesReport.Interest;
                
                feesReports.Add(feesReport);
            }

            return feesReports;
        }
    }
}
