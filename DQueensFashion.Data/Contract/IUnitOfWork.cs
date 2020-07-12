using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Data.Contract
{
    public interface IUnitOfWork
    {
        IProductRepo ProductRepo { get; set; }
        ICategoryRepo CategoryRepo { get; set; }
        ICustomerRepo CustomerRepo { get; set; }
        IWishListRepo WishListRepo { get; set; }
        ILineItemRepo LineItemRepo { get; set; }
        IOrderRepo OrderRepo { get; set; }
        IReviewRepo ReviewRepo { get; set; }
        IImageRepo ImageRepo { get; set; }
        IMailingListRepo MailingListRepo { get; set; }
        IGeneralValuesRepo GeneralValuesRepo { get; set; }
        IMessageRepo MessageRepo { get; set; }
        void Save();
    }
}
