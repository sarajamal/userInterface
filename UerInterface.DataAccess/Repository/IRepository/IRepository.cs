using System.Linq.Expressions;

namespace Test12.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T - Category 
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? incloudeProperties = null);
        T Get(Expression<Func<T, bool>> filter, string? incloudeProperties = null, bool tracked = false);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);//this for delete many Entity In single Colom May be I need . 
    }
}

