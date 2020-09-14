using Data;
using Data.Models;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public interface ICommissionRepository : IGenericRepository<Commission>
    {
    }
}
