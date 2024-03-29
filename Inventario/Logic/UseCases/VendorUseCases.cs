﻿using AutoMapper;
using Data;
using Infrastructure.Repositories;
using Logic.Dtos;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class VendorUseCases : IVendorUseCases
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        public VendorUseCases(IVendorRepository vendorRepository, IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<VendorDto> Create(Guid userId, VendorForCreationDto vendorForCreationDto)
        {
            var vendor = new Vendor()
            {
                Name = vendorForCreationDto.Name,
                CUIL = vendorForCreationDto.CUIL,
                Phone = vendorForCreationDto.Phone,
                Mail = vendorForCreationDto.Mail,
                //Active = vendorForCreationDto.Active,
                Description = vendorForCreationDto.Description,
                CreatedBy = userId
            };
            vendor = await _vendorRepository.Add(vendor);
            await _vendorRepository.CommitAsync();
            return _mapper.Map<Vendor, VendorDto>(vendor);
        }

        public async Task Delete(Guid userId, Guid id)
        {
            var vendor = await _vendorRepository.Delete(userId, id);
            if (vendor == null)
                throw new KeyNotFoundException($"Vendor with id: {id} not found.");

            await _vendorRepository.CommitAsync();
        }

        public async Task<IEnumerable<VendorDto>> GetAll()
        {
            try
            {
                var vendor = (await _vendorRepository.GetAll()).OrderBy(x => x.Name);
                var vendorDto = _mapper.Map<IEnumerable<Vendor>, IEnumerable<VendorDto>>(vendor);
                return vendorDto;
            }
            catch(Exception e) 
            {
                throw e;
            }
        }

        public async Task<int> GetTotalQty()
        {
            return (await _vendorRepository.GetAll()).Count();
        }

        public async Task<int> GetTotalQtyByFilters(VendorFiltersDto filtersDto)
        {
            var exp = filtersDto.GetExpresion();
            return (await _vendorRepository.GetAll()).AsQueryable().Where(exp).Count();
        }

        public async Task<IEnumerable<VendorDto>> GetByPageAndQty(int skip, int qty)
        {
            var vendors = (await _vendorRepository.GetAll()).OrderByDescending(x => x.Name).Skip(skip).Take(qty);
            return _mapper.Map<IEnumerable<Vendor>, IEnumerable<VendorDto>>(vendors);
        }

        public async Task<IEnumerable<VendorDto>> GetFilteredByPageAndQty(VendorFiltersDto filtersDto, int skip, int qty)
        {
            var vendors = (await _vendorRepository.GetAll())
                .AsQueryable().Where(filtersDto.GetExpresion()).ToList().OrderByDescending(x => x.Name).Skip(skip).Take(qty);
            return _mapper.Map<IEnumerable<Vendor>, IEnumerable<VendorDto>>(vendors);
        }

        public async Task<VendorDto> GetOne(Guid id)
        {
            var vendor = await _vendorRepository.GetById(id);
            if (vendor == null)
                throw new KeyNotFoundException($"Vendor with id: {id} not found.");

            return _mapper.Map<Vendor, VendorDto>(vendor);
        }

        public async Task Update(Guid id, VendorDto vendorDto)
        {
            var vendor = await _vendorRepository.GetById(id);
            vendor.Name = vendorDto.Name;
            vendor.CUIL = vendorDto.CUIL;
            vendor.Phone = vendorDto.Phone;
            vendor.Mail = vendorDto.Mail;
            //vendor.Active = vendorDto.Active;
            vendor.Description = vendorDto.Description;
            vendor.LastModificationBy = vendorDto.LastModificationBy;

            await _vendorRepository.Update(vendor);
            await _vendorRepository.CommitAsync();
        }
    }
}
