using System.Collections.Generic;

namespace DevCon2011.Specifications.Data
{
    public interface IRepository
    {
        IEnumerable<TEntity> Query<TEntity>(Specification<TEntity> specification) where TEntity : class;

        // . . .
    }
}