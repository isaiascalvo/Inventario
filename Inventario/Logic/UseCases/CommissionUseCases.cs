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

namespace Logic
{
    public class CommissionUseCases : ICommissionUseCases
    {
        private readonly ICommissionRepository _commissionRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public CommissionUseCases(ICommissionRepository commissionRepository, IVendorRepository vendorRepository, IClientRepository clientRepository, IMapper mapper)
        {
            _commissionRepository = commissionRepository;
            _vendorRepository = vendorRepository;
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<CommissionDto> Create(Guid userId, CommissionForCreationDto commissionForCreationDto)
        {
            var vendor = await _vendorRepository.GetById(commissionForCreationDto.VendorId);
            if (vendor == null)
                throw new KeyNotFoundException($"Vendor with id: {commissionForCreationDto.VendorId} not found.");

            Client client = null;
            if (commissionForCreationDto.ClientId.HasValue)
            {
                client = await _clientRepository.GetById(commissionForCreationDto.ClientId.Value);
                if (client == null)
                    throw new KeyNotFoundException($"Client with id: {commissionForCreationDto.ClientId} not found.");
            }

            var commission = new Commission()
            {
                VendorId = commissionForCreationDto.VendorId,
                ClientId = commissionForCreationDto.ClientId,
                ClientName = client != null ? client.Name + " " + client.Lastname : commissionForCreationDto.ClientName,
                Product = commissionForCreationDto.Product,
                Price = commissionForCreationDto.Price,
                PaymentType = commissionForCreationDto.PaymentType,
                Date = commissionForCreationDto.Date.ToLocalTime(),
                Value = commissionForCreationDto.Value,
                CreatedBy = userId
            };

            commission = await _commissionRepository.Add(commission);
            await _commissionRepository.CommitAsync();
            return _mapper.Map<Commission, CommissionDto>(commission);
        }

        public async Task Delete(Guid userId, Guid id)
        {
            var commission = await _commissionRepository.Delete(userId, id);
            if (commission == null)
                throw new KeyNotFoundException($"Client with id: {id} not found.");

            await _commissionRepository.CommitAsync();
        }

        public async Task<IEnumerable<CommissionDto>> GetAll()
        {
            var commission = (await _commissionRepository.GetAll(x => x.Include( c => c.Vendor))).OrderByDescending(x => x.Date).ThenBy(x => x.Vendor.Name);
            return _mapper.Map<IEnumerable<Commission>, IEnumerable<CommissionDto>>(commission);
        }

        public async Task<int> GetTotalQty()
        {
            return (await _commissionRepository.GetAll()).Count();
        }

        public async Task<int> GetTotalQtyByFilters(CommissionFiltersDto filtersDto)
        {
            var exp = filtersDto.GetExpresion();
            return (await _commissionRepository.GetAll()).AsQueryable().Where(exp).Count();
        }

        public async Task<IEnumerable<CommissionDto>> GetByPageAndQty(int skip, int qty)
        {
            var commissions = (await _commissionRepository.GetAll(x => x.Include(c => c.Vendor))).OrderByDescending(x => x.Date).ThenBy(x => x.Vendor.Name).Skip(skip).Take(qty);
            return _mapper.Map<IEnumerable<Commission>, IEnumerable<CommissionDto>>(commissions);
        }

        public async Task<IEnumerable<CommissionDto>> GetFilteredByPageAndQty(CommissionFiltersDto filtersDto, int skip, int qty)
        {
            var commissions = (await _commissionRepository.GetAll(x => x.Include(c => c.Vendor)))
                .AsQueryable().Where(filtersDto.GetExpresion()).ToList().OrderByDescending(x => x.Date).ThenBy(x => x.Vendor.Name).Skip(skip).Take(qty);
            return _mapper.Map<IEnumerable<Commission>, IEnumerable<CommissionDto>>(commissions);
        }



        public async Task<CommissionDto> GetOne(Guid id)
        {
            var commission = await _commissionRepository.GetById(id, x => x.Include(c => c.Vendor));
            if (commission == null)
                throw new KeyNotFoundException($"Commission with id: {id} not found.");

            return _mapper.Map<Commission, CommissionDto>(commission);
        }

        public async Task Update(Guid id, CommissionDto commissionDto)
        {
            var commission = await _commissionRepository.GetById(id);
            if (commission == null)
                throw new KeyNotFoundException($"Commission with id: {id} not found.");
            
            var vendor = await _vendorRepository.GetById(commissionDto.VendorId);
            if(vendor == null)
                throw new KeyNotFoundException($"Vendor with id: {commissionDto.VendorId} not found.");
            
            Client client = null;
            if (commissionDto.ClientId.HasValue)
            {
                client = await _clientRepository.GetById(commissionDto.ClientId.Value);
                if (client == null)
                    throw new KeyNotFoundException($"Client with id: {commissionDto.ClientId} not found.");
            }

            commission.VendorId = commissionDto.VendorId;
            commission.ClientId = commissionDto.ClientId;
            commission.ClientName = client != null ? client.Name + " " + client.Lastname : commissionDto.ClientName;
            commission.Product = commissionDto.Product;
            commission.Price = commissionDto.Price;
            commission.PaymentType = commissionDto.PaymentType;
            commission.Date = commissionDto.Date.ToLocalTime();
            commission.Value = commissionDto.Value;
            commission.LastModificationBy = commissionDto.LastModificationBy;

            await _commissionRepository.Update(commission);
            await _commissionRepository.CommitAsync();
        }
    }
}
