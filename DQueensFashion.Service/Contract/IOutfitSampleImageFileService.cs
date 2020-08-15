using DQueensFashion.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Contract
{
    public interface IOutfitSampleImageFileService
    {
        int Count();
        void AddOutfitSampleImageFile(IEnumerable<OutfitSampleImageFile> outfitSampleImageFile);
        IEnumerable<OutfitSampleImageFile> GetAllImageFiles();
        IEnumerable<OutfitSampleImageFile> GetAllImageMainFiles();
    }
}
