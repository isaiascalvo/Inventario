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
    public class ClientUseCases : IClientUseCases
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public ClientUseCases(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<ClientDto> Create(ClientForCreationDto clientForCreationDto)
        {
            var client = new Client()
            {
                Name = clientForCreationDto.Name,
                LastName = clientForCreationDto.LastName,
                Dni = clientForCreationDto.Dni,
                Phone = clientForCreationDto.Phone,
                Mail = clientForCreationDto.Mail,
                Active = clientForCreationDto.Active,
                Birthdate = clientForCreationDto.Birthdate
            };

            return _mapper.Map<Client, ClientDto>(await _clientRepository.Add(client));
        }

        public async Task Delete(Guid id)
        {
            var client = await _clientRepository.Delete(id);
            if (client == null)
                throw new KeyNotFoundException($"Client with id: {id} not found.");
        }

        public async Task<IEnumerable<ClientDto>> GetAll()
        {
            var client = await _clientRepository.GetAll();
            var clientDto = _mapper.Map<IEnumerable<Client>, IEnumerable<ClientDto>>(client);
            return clientDto;
        }

        public async Task<ClientDto> GetOne(Guid id)
        {
            var client = await _clientRepository.GetById(id);
            if (client == null)
                throw new KeyNotFoundException($"Client with id: {id} not found.");

            return _mapper.Map<Client, ClientDto>(client);
        }

        public async Task Update(Guid id, ClientDto clientForCreationDto)
        {
            var client = await _clientRepository.GetById(id);
            if (client == null)
                throw new KeyNotFoundException($"Client with id: {id} not found.");

            client.Name = clientForCreationDto.Name;
            client.LastName = clientForCreationDto.LastName;
            client.Dni = clientForCreationDto.Dni;
            client.Phone = clientForCreationDto.Phone;
            client.Mail = clientForCreationDto.Mail;
            client.Active = clientForCreationDto.Active;
            client.Birthdate = clientForCreationDto.Birthdate;
            await _clientRepository.Update(client);
        }
    }
}
