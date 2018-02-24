using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Abstract
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// 
        /// </summary>
        IQueryable<Product> Products { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        void SaveProduct(Product product);
    }
}
