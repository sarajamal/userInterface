using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Test12.DataAccess.Data;
using Test12.DataAccess.Repository.IRepository;


namespace Test12.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet; //because is generic class I cant do (_context.PreparationList.Add) I need something public.
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
            _context.Preparations.Include(u => u.componentsCountPrint).Include(t => t.toolsCountPrint).Include(s => s.stepsCountPrint);
            _context.PreparationIngredients.Include(u => u.Preparation);
            _context.PreparationTools.Include(u => u.Preparation);
            _context.PreparationSteps.Include(u => u.Preparation);
            _context.Production.Include(u => u.component2).Include(t => t.toolsCountPrint2).Include(s => s.stepsCountPrint2);
            _context.ProductionIngredients.Include(u => u.Production);
            _context.ProductionTools.Include(u => u.Production);
            _context.ProductionSteps.Include(u => u.Production);
            _context.Cleaning.Include(u => u.Brand);
            _context.CleaningSteps.Include(u => u.Cleaning);
            _context.DevicesAndTools.Include(u => u.Brand);
            _context.FoodStuffs.Include(u => u.Brand);
            _context.ReadyProducts.Include(u => u.Brand);
            _context.Brands.Include(u => u.User);

        }


        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? incloudeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = dbSet;

            }
            else
            {
                query = dbSet.AsNoTracking();
            }
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(incloudeProperties))
            {
                foreach (var includeProp in incloudeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? incloudeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(incloudeProperties))
            {
                foreach (var includeProp in incloudeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }
    }
}


