using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System;
using System.Linq;

namespace SportsStore.Domain.Concrete
{
    public class EFProductRepository : IProductRepository//, IDisposable
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

        public void SaveProduct(Product product)
        {
            if(product.ProductID == 0)
            {
                _context.Products.Add(product);
            }
            _context.SaveChanges();
        }
    }
}
