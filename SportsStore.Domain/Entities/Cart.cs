using System;
using System.Collections.Generic;
using System.Linq;


namespace SportsStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> _lineCollection = new List<CartLine>();

        public void AddItem(Product product, int quantity)
        {
            CartLine line = _lineCollection.Where(p => p.Product.ProductID == product.ProductID)
                                           .FirstOrDefault();

            if(line == null && quantity != 0)
            {
                _lineCollection.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else if(line != null)
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Product product)
        {
            _lineCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);
        }

        public decimal ComputeTotalValue()
        {
            return _lineCollection.Sum(e => e.ComputeValue());
        }

        public void Clear()
        {
            _lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return _lineCollection; }
        }
    }
}
