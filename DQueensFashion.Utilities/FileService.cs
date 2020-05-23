using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DQueensFashion.Utilities
{
    public class FileService
    {
        public static string GetFileName(HttpPostedFileBase file)
        {

            if (file == null)
                throw new Exception();

            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);

            fileName = fileName + DateTime.Now.ToString("yyyymmddhhmmssfff") + extension;
            return fileName;

        }

        public static void SaveImage(HttpPostedFileBase file, string fileName)
        {
            try
            {
                if (file == null)
                    throw new Exception();

                string ext = Path.GetExtension(file.FileName);
                if (!CheckIfFileIsAnImage(ext.ToLower()))
                    throw new Exception();

                if (!CheckFileSize(file))
                    throw new Exception();

                file.SaveAs(fileName);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void SaveTextFile(HttpPostedFileBase file, string fileName)
        {

            try
            {
                if (file == null)
                    throw new Exception();

                string ext = Path.GetExtension(file.FileName);
                if (!CheckIfFileIsADoc(ext.ToLower()))
                    throw new Exception();

                if (!CheckFileSize(file))
                    throw new Exception();

                file.SaveAs(fileName);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void DeleteFile(string path)
        {
            //string path = Request.MapPath(filePath);
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }

        private static bool CheckIfFileIsAnImage(string extension)
        {
            if (extension == ".jpeg" || extension == ".gif" || extension == ".png" || extension == ".jpg" || extension == ".ico")
                return true;

            return false;
        }

        private static bool CheckIfFileIsADoc(string extension)
        {
            if (extension == ".doc" || extension == ".pdf" || extension == ".docx" || extension == ".txt")
                return true;

            return false;
        }

        private static bool CheckFileSize(HttpPostedFileBase file)
        {
            if (file.ContentLength > (4 * 1000 * 1024))
                return false;

            return true;

        }
    }
}
