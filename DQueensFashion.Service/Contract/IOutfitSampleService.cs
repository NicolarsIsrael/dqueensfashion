using DQueensFashion.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Contract
{
    public interface IOutfitSampleService
    {
        int GetCount();
        void AddOutfitSample(OutfitSample outfitSample);
        IEnumerable<OutfitSample> GetAll();
        OutfitSample GetOutfitSampleById(int id);
        void DeleteOutfitSampleFromDb(OutfitSample outfitSample);
    }
}
