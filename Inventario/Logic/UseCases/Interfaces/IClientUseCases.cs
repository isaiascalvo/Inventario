﻿using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IClientUseCases
    {
        Task<ClientDto> GetOne(Guid id);
        Task<IEnumerable<ClientDto>> GetAll();
        Task Delete(Guid id);
        Task<ClientDto> Create(ClientForCreationDto diseaseDto);
        Task Update(Guid id, ClientDto diseaseDto);
    }
}