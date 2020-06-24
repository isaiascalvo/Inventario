using AutoMapper;
using Data;
using Infrastructure.Repositories;
using Logic.Dtos;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
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

        public async Task<VendorDto> Create(VendorForCreationDto vendorForCreationDto)
        {
            var vendor = new Vendor()
            {
                Name = vendorForCreationDto.Name,
                CUIL = vendorForCreationDto.CUIL,
                Phone = vendorForCreationDto.Phone,
                Mail = vendorForCreationDto.Mail,
                Active = vendorForCreationDto.Active,
                Description = vendorForCreationDto.Description
            };

            return _mapper.Map<Vendor, VendorDto>(await _vendorRepository.Add(vendor));
        }

        public async Task Delete(Guid id)
        {
            var vendor = await _vendorRepository.Delete(id);
            if (vendor == null)
                throw new KeyNotFoundException($"Vendor with id: {id} not found.");
        }

        public async Task<IEnumerable<VendorDto>> GetAll()
        {
            try
            {
                var vendor = await _vendorRepository.GetAll();
                var vendorDto = _mapper.Map<IEnumerable<Vendor>, IEnumerable<VendorDto>>(vendor);
                return vendorDto;
            }
            catch(Exception e) 
            {
                throw e;
            }
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
            vendor.Active = vendorDto.Active;
            vendor.Description = vendorDto.Description;

            await _vendorRepository.Update(vendor);
        }
    }
}
