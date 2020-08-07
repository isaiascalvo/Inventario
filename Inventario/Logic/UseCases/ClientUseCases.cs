using AutoMapper;
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
    public class ClientUseCases : IClientUseCases
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public ClientUseCases(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<ClientDto> Create(Guid userId, ClientForCreationDto clientForCreationDto)
        {
            var client = new Client()
            {
                Name = clientForCreationDto.Name,
                Lastname = clientForCreationDto.Lastname,
                Dni = clientForCreationDto.Dni,
                Phone = clientForCreationDto.Phone,
                Mail = clientForCreationDto.Mail,
                Active = clientForCreationDto.Active,
                Birthdate = clientForCreationDto.Birthdate,
                CreatedBy = userId
            };

            client = await _clientRepository.Add(client);
            await _clientRepository.CommitAsync();
            return _mapper.Map<Client, ClientDto>(client);
        }

        public async Task Delete(Guid userId, Guid id)
        {
            var client = await _clientRepository.Delete(userId, id);
            if (client == null)
                throw new KeyNotFoundException($"Client with id: {id} not found.");

            await _clientRepository.CommitAsync();
        }

        public async Task<IEnumerable<ClientDto>> GetAll()
        {
            var client = await _clientRepository.GetAll();
            var clientDto = _mapper.Map<IEnumerable<Client>, IEnumerable<ClientDto>>(client);
            return clientDto;
        }
        public async Task<IEnumerable<ClientDto>> GetByFilters(ClientFiltersDto filters)
        {
            var tt = filters.GetExpresion();
            var clients = (await _clientRepository.GetAll()).AsQueryable().Where(tt).ToList().OrderBy(x => x.Name);
            return _mapper.Map<IEnumerable<Client>, IEnumerable<ClientDto>>(clients);
        }


        public async Task<ClientDto> GetOne(Guid id)
        {
            var client = await _clientRepository.GetById(id);
            if (client == null)
                throw new KeyNotFoundException($"Client with id: {id} not found.");

            return _mapper.Map<Client, ClientDto>(client);
        }

        public async Task Update(Guid id, ClientDto clientDto)
        {
            var client = await _clientRepository.GetById(id);
            if (client == null)
                throw new KeyNotFoundException($"Client with id: {id} not found.");

            client.Name = clientDto.Name;
            client.Lastname = clientDto.Lastname;
            client.Dni = clientDto.Dni;
            client.Phone = clientDto.Phone;
            client.Mail = clientDto.Mail;
            client.Active = clientDto.Active;
            client.Birthdate = clientDto.Birthdate;
            client.LastModificationBy = clientDto.LastModificationBy;

            await _clientRepository.Update(client);
            await _clientRepository.CommitAsync();
        }
    }
}
