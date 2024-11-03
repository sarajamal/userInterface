using Test12.DataAccess.Data;
using Test12.DataAccess.Repository.IRepository;
using Test12.Models.Models;

namespace Test12.DataAccess.Repository
{
    public class MainsectionRepository : Repository<MainSections>, IMainsectionRepository
    {

        private readonly ApplicationDbContext _context;
        public MainsectionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(MainSections obj)
        {
            var objFormDb = _context.MainSections.FirstOrDefault(u => u.MainSectionsID == obj.MainSectionsID);
            if (objFormDb != null)
            {
                // Update properties for Step 1
                objFormDb.SectionsName = obj.SectionsName;
                objFormDb.MainSectionsOrder = obj.MainSectionsOrder;
                objFormDb.IsChecked = obj.IsChecked;

                if (obj.SectionsImage != null)
                {
                    objFormDb.SectionsImage = obj.SectionsImage;
                }
                // Update properties for Step 2
                _context.SaveChanges();

                //_context.Update(obj);

            }
        }

    }
}
