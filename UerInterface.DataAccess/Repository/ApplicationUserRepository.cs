using Test12.DataAccess.Data;
using Test12.DataAccess.Repository.IRepository;
using Test12.Models.Models;
using Test12.Models.Models.Preparation;
using Test12.Models.Models.trade_mark;

namespace Test12.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

