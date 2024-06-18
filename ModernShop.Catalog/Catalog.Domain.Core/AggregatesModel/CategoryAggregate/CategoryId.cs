﻿using Catalog.Domain.Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Core
{
    public sealed class CategoryId : StronglyTypedId<CategoryId>
    {
        public CategoryId(Guid value) : base(value)
        {
        }
    }
}
