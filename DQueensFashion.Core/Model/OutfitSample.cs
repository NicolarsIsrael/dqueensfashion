using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Core.Model
{
    public class OutfitSample : Entity
    {
        public string OutfitName { get; set; }
        public IEnumerable<OutfitSampleImageFile> Images { get; set; }
    }
}
