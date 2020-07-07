using DQueensFashion.Core.Model;
using DQueensFashion.Data.Contract;
using DQueensFashion.Service.Contract;
using DQueensFashion.Utilities;
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

        public void UpdateGeneralValues(GeneralValues generalValues)
        {
            if (!ValidateGeneralValues(generalValues))
                throw new Exception();

            uow.GeneralValuesRepo.Update(generalValues);
            uow.Save();
        }

        public GeneralValues GetGeneralValues()
        {
            return uow.GeneralValuesRepo.Get(AppConstant.GeneralValId);
        }

        private bool ValidateGeneralValues(GeneralValues generalValues)
        {
            if (generalValues == null)
                return false;

            if (generalValues.NewsLetterSubscriptionDiscount < 0 || generalValues.NewsLetterSubscriptionDiscount > 100)
                return false;

            return true;
        }
    }
}
