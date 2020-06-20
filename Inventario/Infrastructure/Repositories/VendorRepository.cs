using Data;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class VendorRepository : GenericRepository<Vendor>, IVendorRepository
    {
        public VendorRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
