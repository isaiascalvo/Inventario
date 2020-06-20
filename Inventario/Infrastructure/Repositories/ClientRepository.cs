using Data;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
