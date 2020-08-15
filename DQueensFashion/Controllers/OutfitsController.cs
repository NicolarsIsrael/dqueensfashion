using DQueensFashion.Core.Model;
using DQueensFashion.Models;
using DQueensFashion.Service.Contract;
using DQueensFashion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DQueensFashion.Controllers
{
    public class OutfitsController : Controller
    {
        private readonly IOutfitSampleService _outfitsampleService;
        private readonly IOutfitSampleImageFileService _outfitSampleImageFileService;
        public OutfitsController(IOutfitSampleService outfitsampleService, IOutfitSampleImageFileService outfitSampleImageFileService)
        {
            _outfitsampleService = outfitsampleService;
            _outfitSampleImageFileService = outfitSampleImageFileService;
        }
        // GET: Outfits
        public ActionResult Index()
        {
            var allImages = _outfitSampleImageFileService.GetAllImageMainFiles().ToList();
            IEnumerable<OutfitSampleViewModel> outfits = _outfitsampleService.GetAll()
                .Select(sample => new OutfitSampleViewModel()
                {
                    SampleId = sample.Id,
                    SampleName = sample.OutfitName,
                    MainPath = allImages.Where(image => image.OutfitSampleId == sample.Id).Count() < 1 ?
                          AppConstant.DefaultProductImage :
                          allImages.Where(image => image.OutfitSampleId == sample.Id).FirstOrDefault().ImagePath,
                    DateCreated = sample.DateCreated,
                }).OrderByDescending(sample => sample.DateCreated);

            return View(outfits);
        }

        public ActionResult ViewOutfit(int id = 0)
        {
            var images = _outfitSampleImageFileService.GetAllImageFiles().Where(sample => sample.OutfitSampleId == id);

            OutfitSample outfitSample = _outfitsampleService.GetOutfitSampleById(id);
            if (outfitSample == null)
                throw new Exception();

            OutfitSampleViewModel outfitsampleModel = new OutfitSampleViewModel()
            {
                SampleName = outfitSample.OutfitName,
                OtherPaths = images.Select(s => new OutfitSampleImageFileViewModel()
                {
                    ImagePath = s.ImagePath,
                }),
            };
            return View(outfitsampleModel);
        }
    }
}