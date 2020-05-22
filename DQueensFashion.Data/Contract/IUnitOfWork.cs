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
        void Save();
    }
}
