﻿using Data;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public interface IProductEntryRepository : IGenericRepository<ProductEntry>
    {
    }
}
