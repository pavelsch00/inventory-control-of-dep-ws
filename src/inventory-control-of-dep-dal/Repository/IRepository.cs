using inventory_control_of_dep_dal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory_control_of_dep_dal.Repository
{
    public interface IRepository<TEntity>
           where TEntity : class, IHasBasicId
    {
        public IQueryable<TEntity> GetAll();

        public Task<TEntity> GetById(int id);

        public Task<int> Create(TEntity item);

        public Task Update(TEntity item);

        public Task Delete(int id);
    }
}
