using DQueensFashion.Core.Model;
using DQueensFashion.Data.Contract;
using DQueensFashion.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Implementation
{
    public class LineItemService : ILineItemService
    {
        IUnitOfWork uow;
        public LineItemService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public int GetLineItemsCount()
        {
            return uow.LineItemRepo.Count();
        }

        public LineItem GetLineItemById(int id)
        {
            return uow.LineItemRepo.GetLineItemWithRelationships(id);
        }

    }
}
