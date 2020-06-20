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
    public class DiseaseUseCases : IDiseaseUseCases
    {
        private readonly IDiseaseRepository _diseaseRepository;
        private readonly IMapper _mapper;

        public DiseaseUseCases(IDiseaseRepository diseaseRepository, IMapper mapper)
        {
            _diseaseRepository = diseaseRepository;
            _mapper = mapper;
        }

        public async Task<DiseaseDto> Create(DiseaseForCreationDto diseaseForCreationDto)
        {
            var disease = new Client()
            {
                Name = diseaseForCreationDto.Name,
                Description = diseaseForCreationDto.Description
            };

            return _mapper.Map<Client, DiseaseDto>(await _diseaseRepository.Add(disease));
        }

        public async Task Delete(Guid id)
        {
            var disease = await _diseaseRepository.Delete(id);
            if (disease == null)
                throw new KeyNotFoundException($"Disease with id: {id} not found.");
        }

        public async Task<IEnumerable<DiseaseDto>> GetAll()
        {
            var disease = await _diseaseRepository.GetAll();
            var diseaseDto = _mapper.Map<IEnumerable<Client>, IEnumerable<DiseaseDto>>(disease);
            return diseaseDto;
        }

        public async Task<DiseaseDto> GetOne(Guid id)
        {
            var disease = await _diseaseRepository.GetById(id);
            if (disease == null)
                throw new KeyNotFoundException($"Disease with id: {id} not found.");

            return _mapper.Map<Client, DiseaseDto>(disease);
        }

        public async Task Update(Guid id, DiseaseDto diseaseDto)
        {
            var disease = await _diseaseRepository.GetById(id);
            disease.Name = diseaseDto.Name;
            disease.Description = diseaseDto.Description;
            await _diseaseRepository.Update(disease);
        }
    }
}
