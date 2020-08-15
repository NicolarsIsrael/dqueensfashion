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
    public class OutfitSampleService : IOutfitSampleService
    {
        IUnitOfWork uow;
        public OutfitSampleService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public void AddOutfitSample(OutfitSample outfitSample)
        {
            uow.OutfitSampleRepo.Add(outfitSample);
            uow.Save();
        }

        public OutfitSample GetOutfitSampleById(int id)
        {
            return uow.OutfitSampleRepo.Get(id);
        }

        public void DeleteOutfitSampleFromDb(OutfitSample outfitSample)
        {
            uow.OutfitSampleRepo.DeleteFromDb(outfitSample);
            uow.Save();
        }

        public IEnumerable<OutfitSample> GetAll()
        {
            return uow.OutfitSampleRepo.GetAll();
        }

        public int GetCount()
        {
            return uow.OutfitSampleRepo.Count();
        }
    }
}