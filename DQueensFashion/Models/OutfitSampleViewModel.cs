using DQueensFashion.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DQueensFashion.Models
{
    public class OutfitSampleViewModel
    {
        public int SampleId { get; set; }
        public string MainPath { get; set; }
        public string SampleName { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<OutfitSampleImageFileViewModel> OtherPaths { get; set; }
    }

    public class OutfitSampleImageFileViewModel
    {
        public string ImagePath { get; set; }
    }

    public class AddOutfitSampleViewModel
    {
        [Required(ErrorMessage ="Name is required")]
        [Display(Name ="Outfit sample name")]
        [BeginWIthAlphaNumeric(ErrorMessage ="Should begin with alphabeth or number")]
        public string SampleName { get; set; }
    }
}