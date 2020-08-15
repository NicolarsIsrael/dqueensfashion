using DQueensFashion.Data.Contract;
using DQueensFashion.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Implementation
{
    public class OutfitSampleImageFileService : IOutfitSampleImageFileService
    {
        IUnitOfWork uow;
        public OutfitSampleImageFileService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public int Count()
        {
            return uow.OutfitSampleImageFileRepo.Count();
        }
    }
}
