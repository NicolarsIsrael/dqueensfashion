using DQueensFashion.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Contract
{
    public interface ILineItemService
    {
        int GetLineItemsCount();
        LineItem GetLineItemById(int id);
    }
}
