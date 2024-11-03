using Test12.DataAccess.Data;
using Test12.DataAccess.Repository.IRepository;
using Test12.Models.Models.Food;

namespace Test12.DataAccess.Repository
{
    public class FoodRepository : Repository<FoodStuffs>, IFoodRepository
    {
        private readonly ApplicationDbContext _context;
        public FoodRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(FoodStuffs obj)
        {

            var objFormDb = _context.FoodStuffs.FirstOrDefault(u => u.FoodStuffsID == obj.FoodStuffsID);
            if (objFormDb != null)
            {

                objFormDb.FoodStuffsName = obj.FoodStuffsName;
                objFormDb.FoodStuffsOrder = obj.FoodStuffsOrder;
                objFormDb.FoodStuffsNum = obj.FoodStuffsNum;
                if (obj.FoodStuffsImage != null)
                {
                    objFormDb.FoodStuffsImage = obj.FoodStuffsImage;
                }
                _context.SaveChanges();
            }

        }
        public int GetLastStepId()
        {
            // If there are no entries in the table, return 0 or an appropriate default value
            if (!_context.FoodStuffs.Any())
            {
                return 0;
            }
            // Retrieve and return the max PrepStepsID
            return _context.FoodStuffs.Max(p => p.FoodStuffsID);
        }

    }
}
