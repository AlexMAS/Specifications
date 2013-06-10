using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;

namespace DevCon2011.Specifications.Data
{
    public class EntityFrameworkRepository : IRepository
    {
        private readonly ObjectContext _dataContext;

        public EntityFrameworkRepository(ObjectContext dataContext)
        {
            _dataContext = dataContext;
        }


        public IEnumerable<TEntity> Query<TEntity>(Specification<TEntity> specification) where TEntity : class
        {
            return _dataContext.CreateObjectSet<TEntity>().Where(specification).ToArray();
        }
    }
}