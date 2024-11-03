using Test12.DataAccess.Data;
using Test12.DataAccess.Repository.IRepository;
using Test12.Models.Models.trade_mark;

namespace Test12.DataAccess.Repository
{
    public class TredMarketRepository : Repository<Brands>, ITredMarketRepository
    {
        private readonly ApplicationDbContext _context;
        public TredMarketRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Brands obj)
        {
            var objFormDb = _context.Brands.FirstOrDefault(u => u.BrandID == obj.BrandID);
            if (objFormDb != null)
            {
                objFormDb.BrandName = obj.BrandName;
                objFormDb.ClientID = obj.ClientID;

                if (obj.BrandCoverImage != null)
                {
                    objFormDb.BrandCoverImage = obj.BrandCoverImage;
                }
                if (obj.BrandLogoImage != null)
                {
                    objFormDb.BrandLogoImage = obj.BrandLogoImage;
                }
                if (obj.BrandFooterImage != null)
                {
                    objFormDb.BrandFooterImage = obj.BrandFooterImage;
                }
            }
        }


    }

}
