﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Repository.Repository
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
