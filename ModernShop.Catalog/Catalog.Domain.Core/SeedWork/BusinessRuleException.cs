﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Core.SeedWork
{
    public class BusinessRuleException:Exception
    {
        public BusinessRuleException(string message):base(message)
        {

        }
    }
}
