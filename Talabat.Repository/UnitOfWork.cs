using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        private Hashtable repositories;

        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> Complete()
            => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await _dbContext.DisposeAsync();

        public IGenericRepository<T>? Repository<T>() where T : BaseEntity
        {
            if(repositories is null)
                repositories = new Hashtable();
            var type = typeof(T).Name;
            if(!repositories.ContainsKey(type))
            {
                var repo = new GenericRepository<T>(_dbContext);
                repositories.Add(type, repo);
            }
            return repositories[type] as IGenericRepository<T>;
        }
    }
}
