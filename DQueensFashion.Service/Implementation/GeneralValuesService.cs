using DQueensFashion.Data.Contract;
using DQueensFashion.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Implementation
{
    public class GeneralValuesService : IGeneralValuesService
    {
        IUnitOfWork uow;
        public GeneralValuesService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public int GetTotalGeneralValuesCount()
        {
            return uow.GeneralValuesRepo.Count();
        }
    }
}
