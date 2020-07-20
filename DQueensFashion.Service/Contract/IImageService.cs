using DQueensFashion.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Contract
{
    public interface IImageService
    {
        void AddImage(ImageFile imageFile);
        void AddRangeImages(List<ImageFile> imageFiles);
        IEnumerable<ImageFile> GetImageFilesForProduct(int productId);
        ImageFile GetMainImageForProduct(int productId);
        ImageFile GetImageById(int imageId);
        void DeleteImage(ImageFile image);
        IEnumerable<ImageFile> GetAllImageFiles();
        IEnumerable<ImageFile> GetAllImageMainFiles();
    }
}
