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
        public ICustomerRepo CustomerRepo { get; set; }
        public IWishListRepo WishListRepo { get; set; }
        public ILineItemRepo LineItemRepo { get; set; }
        public IOrderRepo OrderRepo { get; set; }
        public IReviewRepo ReviewRepo { get; set; }
        public IImageRepo ImageRepo { get; set; }
        public IMailingListRepo MailingListRepo { get; set; }
        public IGeneralValuesRepo GeneralValuesRepo { get; set; }
        public IMessageRepo MessageRepo { get; set; }

        public UnitOfWork(System.Data.Entity.DbContext _context,IProductRepo _productRepo, ICategoryRepo _categoryRepo,ICustomerRepo _customerRepo,
            IWishListRepo _wishListRepo, ILineItemRepo _lineItemRepo, IOrderRepo _orderRepo, IReviewRepo _reviewRepo, IImageRepo _imageRepo,
            IMailingListRepo _mailingListRepo, IGeneralValuesRepo _generalValuesRepo, IMessageRepo _messageRepo)
        {
            context = _context;
            ProductRepo = _productRepo;
            CategoryRepo = _categoryRepo;
            CustomerRepo = _customerRepo;
            WishListRepo = _wishListRepo;
            LineItemRepo = _lineItemRepo;
            OrderRepo = _orderRepo;
            ReviewRepo = _reviewRepo;
            ImageRepo = _imageRepo;
            MailingListRepo = _mailingListRepo;
            GeneralValuesRepo = _generalValuesRepo;
            MessageRepo = _messageRepo;
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
