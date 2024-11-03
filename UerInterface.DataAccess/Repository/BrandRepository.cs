using Test12.DataAccess.Data;
using Test12.DataAccess.Repository.IRepository;
using Test12.Models.Models.Preparation;
using Test12.Models.Models.trade_mark;

namespace Test12.DataAccess.Repository
{
    public class BrandRepository : Repository<Brands>, IBrandRepository
    {
        private readonly ApplicationDbContext _context;
        public BrandRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Brands obj)
        {
            var objFormDb = _context.Brands.FirstOrDefault(u => u.BrandID == obj.BrandID);
            if (objFormDb != null)
            {

                objFormDb.BrandName = obj.BrandName;
                objFormDb.UserId = obj.UserId;
                objFormDb.Date1 = obj.Date1;

                if (obj.BrandFooterImage != null)
                {
                    objFormDb.BrandFooterImage = obj.BrandFooterImage;
                }
                if (obj.BrandCoverImage != null)
                {
                    objFormDb.BrandCoverImage = obj.BrandCoverImage;
                }
                if (obj.BrandLogoImage != null)
                {
                    objFormDb.BrandLogoImage = obj.BrandLogoImage;
                }
                _context.SaveChanges();
            }
            //_context.Update(obj);

        }


    }
}

