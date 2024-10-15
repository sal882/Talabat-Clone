using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext) // Ask the CLR to Inject Obj from Store Context implicitly
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //if (typeof(T) == typeof(Product))
            //    return (IEnumerable<T>)await _dbContext.Set<Product>()
            //        .Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync();
            return await _dbContext.Set<T>().ToListAsync();
        }


        public async Task<T> GetByIdAsync(int id)
            => await _dbContext.Set<T>().FindAsync(id);

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
            => await ApplayGetQuery(spec).ToListAsync();

        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> spec)
            => await ApplayGetQuery(spec).FirstOrDefaultAsync();

        //Get count of All [product that apply specific filteration]
        public async Task<int> GetCountWitheSpecAsync(ISpecification<T> spec)
        {
            return await ApplayGetQuery(spec).CountAsync();
        }
        //Use this Function to apply DRY Principle
        private IQueryable<T> ApplayGetQuery(ISpecification<T> spec)
            => SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);

        public async Task Add(T entity)
            => await _dbContext.Set<T>().AddAsync(entity);

        public void Update(T entity)
            => _dbContext.Set<T>().Update(entity);

        public void Delete(T entity)
                    => _dbContext.Set<T>().Remove(entity);

    }
}
