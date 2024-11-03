
using Test12.Models.Models.trade_mark;

namespace Test12.DataAccess.Repository.IRepository
{
    public interface IBrandRepository : IRepository<Brands>
    {

        void Update(Brands obj);



    }
}
