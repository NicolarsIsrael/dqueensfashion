using DQueensFashion.Core.Model;
using DQueensFashion.Data.Contract;
using DQueensFashion.Service.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Implementation
{
    public class ImageService : IImageService
    {
        IUnitOfWork uow;
        public ImageService(IUnitOfWork _uow)
        {
            uow = _uow;
        }


        public void AddImage(ImageFile imageFile)
        {
            if (!ValidateImageFileDetails(imageFile))
                throw new Exception();

            uow.ImageRepo.Add(imageFile);
            uow.Save();
        }

        public void AddRangeImages(List<ImageFile> imageFiles)
        {
            foreach(var imageFile in imageFiles)
            {
                if (!ValidateImageFileDetails(imageFile))
                    throw new Exception();
            }

            uow.ImageRepo.AddRange(imageFiles);
            uow.Save();
        }

        public void DeleteImage(ImageFile image)
        {
            if (image!=null)
            {
                uow.ImageRepo.DeleteFromDb(image);
                uow.Save(); 
            }
        }

        public IEnumerable<ImageFile> GetAllImageFiles()
        {
            return uow.ImageRepo.GetAll().OrderBy(image => image.DateCreated);
        }

        public ImageFile GetImageById(int imageId)
        {
            return uow.ImageRepo.Get(imageId);
        }

        public IEnumerable<ImageFile> GetImageFilesForProduct(int productId)
        {
            return uow.ImageRepo.GetAll().Where(image => image.ProductId == productId).ToList();
        }

        public ImageFile GetMainImageForProduct(int productId)
        {
            if (GetImageFilesForProduct(productId).Count() < 1)
                return null;

            return uow.ImageRepo.GetAll().Where(image => image.ProductId == productId)
                .OrderBy(image => image.DateCreated).FirstOrDefault();
        }

        private bool ValidateImageFileDetails(ImageFile imageFile)
        {
            if (imageFile == null)
                return false;

            if (string.IsNullOrEmpty(imageFile.ImagePath))
                return false;

            return true;
        }
    }
}

