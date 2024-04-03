using Demand.Core.Entities;
using System.Linq.Expressions;

namespace Demand.Core.DataAccess.EntityFramework;

public interface IEntityRepository<T> where T : class, IEntity, new()
{
    IList<T> GetAll();
    T Get(Expression<Func<T, bool>> filter);
    IList<T> GetList(Expression<Func<T, bool>> filter = null);
    T? GetFirstOrDefault(Expression<Func<T, bool>> filter);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
