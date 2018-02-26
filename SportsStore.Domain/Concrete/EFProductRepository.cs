using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System;
using System.Linq;

namespace SportsStore.Domain.Concrete
{
    public class EFProductRepository : IProductRepository, IDisposable
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
            else
            {
                var result = _context.Products.FirstOrDefault(p => p.ProductID == product.ProductID);
                _context.Entry(result).CurrentValues.SetValues(product);
            }            
            _context.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; // Per rilevare chiamate ridondanti

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: eliminare lo stato gestito (oggetti gestiti).
                    _context.Dispose();
                }

                // TODO: liberare risorse non gestite (oggetti non gestiti) ed eseguire sotto l'override di un finalizzatore.
                // TODO: impostare campi di grandi dimensioni su Null.

                disposedValue = true;
            }
        }

        // TODO: eseguire l'override di un finalizzatore solo se Dispose(bool disposing) include il codice per liberare risorse non gestite.
        // ~EFProductRepository() {
        //   // Non modificare questo codice. Inserire il codice di pulizia in Dispose(bool disposing) sopra.
        //   Dispose(false);
        // }

        // Questo codice viene aggiunto per implementare in modo corretto il criterio Disposable.
        public void Dispose()
        {
            // Non modificare questo codice. Inserire il codice di pulizia in Dispose(bool disposing) sopra.
            Dispose(true);
            // TODO: rimuovere il commento dalla riga seguente se è stato eseguito l'override del finalizzatore.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
