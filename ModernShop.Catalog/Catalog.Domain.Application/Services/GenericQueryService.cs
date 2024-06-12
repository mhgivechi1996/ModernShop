using Catalog.Application.Contract.Common;
using Catalog.Application.IServices;
using Catalog.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Services
{
    public class GenericQueryService<T> : IGenericQueryService<T> where T : class
    {
        private CatalogContext context;
        public GenericQueryService(CatalogContext context)
        {
            this.context = context;
        }

        public async Task<CatalogActionResult<List<T>>> QueryAsync(GridQueryDto args = null,
            IList<string> fields = null, IList<string> includes = null)
        {
            var actionResult = new CatalogActionResult<List<T>>();

            var query = context.Set<T>().AsQueryable();

            //includes
            if (includes != null)
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }

            //filter
            if (args != null && args.Filtered != null && args.Filtered.Length > 0)
            {
                // var filterExpression = QueryUtility.FilterExpression<T>(args.Filtered[0].column, args.Filtered[0].value);
                for (int i = 0; i < args.Filtered.Length; i++)
                {
                    var filterExpression = QueryUtility.FilterExpression<T>(args.Filtered[i].column, args.Filtered[i].value);
                    if (filterExpression != null)
                        query = query.Where(filterExpression);
                }
            }

            //total count
            var total = await query.CountAsync();

            //sort
            if (args != null && args.Sorted != null && args.Sorted.Length > 0)
            {
                for (int i = 0; i < args.Sorted.Length; i++)
                {
                    query = query.SortMeDynamically(args.Sorted[i].column, args.Sorted[i].desc);
                }
            }


            //projection
            if (fields != null && fields.Count > 0)
                query = query.SelectDynamic(fields);

            var result = await query.Skip((args.Page - 1) * args.Size)
                .Take(args.Size)
                .AsNoTracking()
                .ToListAsync();

            actionResult.Data = result;
            actionResult.Total = total;
            actionResult.Page = args.Page;

            return actionResult;
        }
    }
}
