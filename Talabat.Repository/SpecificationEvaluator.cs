using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        /// <summary>
        /// this function will Build The Dynamic Query based on Two Paramters
        /// </summary>
        /// <param name="entryPoint">this represent the entryPoin of query like _dbContext.Set<T>()</param>
        /// <param name="spec">this represtn the specification of query like where and Includes and others</param>
        /// <returns>the Complete Query which built by dynamic way</returns>
        public static IQueryable<T> GetQuery(IQueryable<T> entryPoint, ISpecification<T> spec)
        {
            var query = entryPoint;

            //Check if the Query needs the where condition or no
            //by checkig if criteria is not null, if not we will add it to query
            if(spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            //Check if Query need Order by asyc or des or not
            //by checkig if orderBy or OredrByDesc is not null, if not we will add it to query
            if(spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);

            if(spec.OrderByDesc is not null) 
                query = query.OrderByDescending(spec.OrderByDesc);

            //this will applay pagination to the products
            if(spec.IsPaginationEnabaled)
                query = query.Skip(spec.Skip).Take(spec.Take);
            ///this will Concat all Include() method to the query with there lamda expression
            query = spec.Includes
                .Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            return query;
        }
    }
}
