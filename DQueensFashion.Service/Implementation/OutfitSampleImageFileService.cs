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
    public class OutfitSampleImageFileService : IOutfitSampleImageFileService
    {
        IUnitOfWork uow;
        public OutfitSampleImageFileService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public void AddOutfitSampleImageFile(IEnumerable<OutfitSampleImageFile> outfitSampleImageFile)
        {
            uow.OutfitSampleImageFileRepo.AddRange(outfitSampleImageFile);
            uow.Save();
        }

        public IEnumerable<OutfitSampleImageFile> GetAllImageFiles()
        {
            return uow.OutfitSampleImageFileRepo.GetAll().OrderBy(image => image.DateCreated);
        }

        public IEnumerable<OutfitSampleImageFile> GetAllImageMainFiles()
        {
            return GetAllImageFiles().GroupBy(image => image.OutfitSampleId)
                .Select(x => x.First()).ToList();
        }
        public int Count()
        {
            return uow.OutfitSampleImageFileRepo.Count();
        }
    }
}
