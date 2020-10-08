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
    public class ClientUseCases : IClientUseCases
    {
        private readonly IClientRepository _clientRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IOwnFeesRepository _ownFeesRepository;
        private readonly IMapper _mapper;

        public ClientUseCases(IClientRepository clientRepository, ISaleRepository saleRepository, IOwnFeesRepository ownFeesRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _saleRepository = saleRepository;
            _ownFeesRepository = ownFeesRepository;
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
                Birthdate = clientForCreationDto.Birthdate.ToLocalTime(),
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
            var clients = (await _clientRepository.GetAll()).OrderBy(x => x.Name).ThenBy(x => x.Lastname);
            return _mapper.Map<IEnumerable<Client>, IEnumerable<ClientDto>>(clients);
        }

        public async Task<IEnumerable<ClientDto>> GetByFilters(ClientFiltersDto filters)
        {
            var clients = (await _clientRepository.Find(filters.GetExpresion())).OrderBy(x => x.Name);
            var clientsDto = _mapper.Map<IEnumerable<Client>, IEnumerable<ClientDto>>(clients);
            foreach (var clientDto in clientsDto)
            {
                clientDto.Debtor = await IsDebtor(clientDto);
            }
            return clientsDto.Where(x => filters.Debtor == null || x.Debtor == filters.Debtor);
        }

        public async Task<int> GetTotalQty()
        {
            return (await _clientRepository.GetAll()).Count();
        }

        public async Task<int> GetTotalQtyByFilters(ClientFiltersDto filtersDto)
        {
            var clients = await _clientRepository.Find(filtersDto.GetExpresion());
            var clientsDto = _mapper.Map<IEnumerable<Client>, IEnumerable<ClientDto>>(clients);
            foreach (var clientDto in clientsDto)
            {
                clientDto.Debtor = await IsDebtor(clientDto);
            }
            return clientsDto.Where(x => filtersDto.Debtor == null || x.Debtor == filtersDto.Debtor).Count();
        }

        public async Task<IEnumerable<ClientDto>> GetByPageAndQty(int skip, int qty)
        {
            var clients = (await _clientRepository.GetAll()).OrderByDescending(x => x.Lastname).ThenByDescending(x => x.Name).Skip(skip).Take(qty);
            var clientsDto = _mapper.Map<IEnumerable<Client>, IEnumerable<ClientDto>>(clients);
            foreach (var clientDto in clientsDto)
            {
                clientDto.Debtor = await IsDebtor(clientDto);
            }
            return clientsDto;
        }

        public async Task<IEnumerable<ClientDto>> GetFilteredByPageAndQty(ClientFiltersDto filtersDto, int skip, int qty)
        {
            var clients = (await _clientRepository.Find(filtersDto.GetExpresion()))
                .OrderByDescending(x => x.Lastname).ThenByDescending(x => x.Name);
            var clientsDto = _mapper.Map<IEnumerable<Client>, IEnumerable<ClientDto>>(clients);
            foreach (var clientDto in clientsDto)
            {
                clientDto.Debtor = await IsDebtor(clientDto);
            }
            return clientsDto.Where(x => filtersDto.Debtor == null || x.Debtor == filtersDto.Debtor).Skip(skip).Take(qty);
        }

        public async Task<ClientDto> GetOne(Guid id)
        {
            var client = await _clientRepository.GetById(id);
            if (client == null)
                throw new KeyNotFoundException($"Client with id: {id} not found.");

            var clientDto = _mapper.Map<Client, ClientDto>(client);
            clientDto.Debtor = await IsDebtor(clientDto);
            return clientDto;
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
            client.Birthdate = clientDto.Birthdate.ToLocalTime();
            client.LastModificationBy = clientDto.LastModificationBy;

            await _clientRepository.Update(client);
            await _clientRepository.CommitAsync();
        }
        private async Task<bool> IsDebtor(ClientDto clientDto)
        {
            var ownFeesIds = (await _saleRepository
                    .Find(
                            x => x.ClientId == clientDto.Id &&
                            x.PaymentType == Util.Enums.ePaymentTypes.OwnFees,
                            x => x.Include(s => s.Payment)
                         )
                    )
                    .Select(x => x.PaymentId);

            var result = (await _ownFeesRepository.Find(x => ownFeesIds.Contains(x.Id), x => x.Include(ow => ow.FeeList))).Select(x => x.FeeList)
                    .Where(
                        x => x.Any(
                            ow => ow.PaymentDate == null &&
                            ow.ExpirationDate.ToLocalTime() <= DateTime.Now.ToLocalTime()
                        )
                    )
                    .Count() > 0;
            return result;
        }
    }
}
