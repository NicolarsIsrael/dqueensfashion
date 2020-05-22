using DQueensFashion.Data.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Data.Implementation
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private System.Data.Entity.DbContext context;// = new DbContext();

        public IProductRepo ProductRepo { get; set; }
        public ICategoryRepo CategoryRepo { get; set; }

        public UnitOfWork(System.Data.Entity.DbContext _context,IProductRepo _productRepo, ICategoryRepo _categoryRepo)
        {
            context = _context;
            ProductRepo = _productRepo;
            CategoryRepo = _categoryRepo;
        }

        public void Save()
        {
            context.SaveChanges();
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
