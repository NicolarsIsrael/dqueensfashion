using DQueensFashion.Core.Model;
using DQueensFashion.Data.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Data.Implementation
{
    public class LineItemRepo : CoreRepo<LineItem>, ILineItemRepo
    {
        public LineItemRepo(DbContext ctx): base(ctx)
        {

        }

        //other functions
        public LineItem GetLineItemWithRelationships(int lineItemId)
        {
            return _dbContext.Set<LineItem>()
                .Include(lineItem => lineItem.Product)
                .Include(lineItem => lineItem.Order)
                .Where(lineItem => lineItem.Id == lineItemId)
                .Where(lineItem => !lineItem.IsDeleted)
                .FirstOrDefault();
        }

    }

}
