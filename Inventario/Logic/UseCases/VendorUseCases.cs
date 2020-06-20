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
        private readonly IVendorRepository _pregnancyRepository;
        private readonly IProductRepository _animalRepository;
        private readonly IMapper _mapper;

        public VendorUseCases(IVendorRepository pregnancyRepository, IProductRepository animalRepository, IMapper mapper)
        {
            _pregnancyRepository = pregnancyRepository;
            _animalRepository = animalRepository;
            _mapper = mapper;
        }

        public async Task<VendorDto> Create(VendorForCreationDto pregnancyForCreationDto)
        {
            var animal = await _animalRepository.GetById(pregnancyForCreationDto.AnimalId);
            if (animal == null)
                throw new KeyNotFoundException($"Animal with id: { pregnancyForCreationDto.ProgenitorId.Value } not found.");

            Product progenitor = null;
            if (pregnancyForCreationDto.ProgenitorId.HasValue)
            {
                progenitor = await _animalRepository.GetById(pregnancyForCreationDto.ProgenitorId.Value);
                if (progenitor == null)
                    throw new KeyNotFoundException($"Progenitor with id: { pregnancyForCreationDto.ProgenitorId.Value } not found.");
            }

            var pregnancy = new Vendor()
            {
                AnimalId = pregnancyForCreationDto.AnimalId,
                Animal = animal,
                ProgenitorId = pregnancyForCreationDto.ProgenitorId,
                Progenitor = progenitor,
                Date = pregnancyForCreationDto.Date,
                Observations = pregnancyForCreationDto.Observations,
                Finished = pregnancyForCreationDto.Finished
            };

            return _mapper.Map<Vendor, VendorDto>(await _pregnancyRepository.Add(pregnancy));
        }

        public async Task Delete(Guid id)
        {
            var pregnancy = await _pregnancyRepository.Delete(id);
            if (pregnancy == null)
                throw new KeyNotFoundException($"Pregnancy with id: {id} not found.");
        }

        public async Task<IEnumerable<VendorDto>> GetAll()
        {
            var pregnancy = await _pregnancyRepository.GetAll();
            var pregnancyDto = _mapper.Map<IEnumerable<Vendor>, IEnumerable<VendorDto>>(pregnancy);
            return pregnancyDto;
        }

        public async Task<VendorDto> GetOne(Guid id)
        {
            var pregnancy = await _pregnancyRepository.GetById(id);
            if (pregnancy == null)
                throw new KeyNotFoundException($"Pregnancy with id: {id} not found.");

            return _mapper.Map<Vendor, VendorDto>(pregnancy);
        }

        public async Task Update(Guid id, VendorDto pregnancyDto)
        {
            var animal = await _animalRepository.GetById(pregnancyDto.AnimalId);
            if (animal == null)
                throw new KeyNotFoundException($"Animal with id: { pregnancyDto.ProgenitorId.Value } not found.");

            Product progenitor = null;
            if (pregnancyDto.ProgenitorId.HasValue)
            {
                progenitor = await _animalRepository.GetById(pregnancyDto.ProgenitorId.Value);
                if (progenitor == null)
                    throw new KeyNotFoundException($"Progenitor with id: { pregnancyDto.ProgenitorId.Value } not found.");
            }

            var pregnancy = await _pregnancyRepository.GetById(id);
            pregnancy.AnimalId = pregnancyDto.AnimalId;
            pregnancy.Animal = animal;
            pregnancy.ProgenitorId = pregnancyDto.ProgenitorId;
            pregnancy.Progenitor = progenitor;
            pregnancy.Date = pregnancyDto.Date;
            pregnancy.Observations = pregnancyDto.Observations;
            pregnancy.Finished = pregnancyDto.Finished;

            await _pregnancyRepository.Update(pregnancy);
        }
    }
}
