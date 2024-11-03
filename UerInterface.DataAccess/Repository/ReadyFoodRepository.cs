using Test12.DataAccess.Data;
using Test12.DataAccess.Repository.IRepository;
using Test12.Models.Models.ReadyFood;

namespace Test12.DataAccess.Repository
{
    public class ReadyFoodRepository : Repository<ReadyProducts>, IReadyFoodRepository
    {
        private readonly ApplicationDbContext _context;
        public ReadyFoodRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ReadyProducts obj)
        {

            var objFormDb = _context.ReadyProducts.FirstOrDefault(u => u.ReadyProductsID == obj.ReadyProductsID);
            if (objFormDb != null)
            {
                objFormDb.ReadyProductsName = obj.ReadyProductsName;
                objFormDb.ReadyProductsOrder = obj.ReadyProductsOrder;
                if (obj.ReadyProductsImage != null)
                {
                    objFormDb.ReadyProductsImage = obj.ReadyProductsImage;
                }
                _context.SaveChanges();
            }

        }
        public int GetLastStepId()
        {
            // If there are no entries in the table, return 0 or an appropriate default value
            if (!_context.ReadyProducts.Any())
            {
                return 0;
            }
            // Retrieve and return the max PrepStepsID
            return _context.ReadyProducts.Max(p => p.ReadyProductsID);
        }

    }
}
