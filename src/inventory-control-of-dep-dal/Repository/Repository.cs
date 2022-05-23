using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

using inventory_control_of_dep_dal.Domain;

namespace inventory_control_of_dep_dal.Repository
{
    internal class Repository<TEntity> : IRepository<TEntity>
            where TEntity : class, IHasBasicId
    {
        private DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        protected DbContext DbContext
        {
            get => _dbContext;
            set
            {
                _dbContext = value ??
                    throw new InvalidOperationException("The DBcontext cannot be null." +
                        "Repository implementation required.");
            }
        }

        public async Task<int> Create(TEntity item)
        {
            DbContext.Set<TEntity>().Add(item);
            await DbContext.SaveChangesAsync();

            return item.Id;
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>().AsNoTracking();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await DbContext.Set<TEntity>()
                          .AsNoTracking()
                          .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Update(TEntity item)
        {
            DbContext.Set<TEntity>().Update(item);
            await DbContext.SaveChangesAsync();
        }
    }
}
