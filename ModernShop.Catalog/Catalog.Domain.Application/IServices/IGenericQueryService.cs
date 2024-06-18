using Catalog.Application.Contract.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.IServices
{
    public interface IGenericQueryService<T> where T : class
    {
        Task<CatalogActionResult<List<T>>> QueryAsync(GridQueryDto args, IList<string> fields = null, IList<string> includes = null);
    }
}
