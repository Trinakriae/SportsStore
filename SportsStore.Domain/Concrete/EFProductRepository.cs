using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System.Linq;

namespace SportsStore.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private EFDbContext _context;

        public EFProductRepository()
        {
            _context = new EFDbContext();
        }

        public EFProductRepository(EFDbContext context)
        {
            _context = context;
        }

        public IQueryable<Product> Products
        {
            get { return _context.Products; }
        }
    }
}
