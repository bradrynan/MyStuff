using MyStuff.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using MyStuff.DAL;

namespace MyStuff.Service
{
    public class ManagePhotosService
    {
        private GalleryContext db = new GalleryContext();
        ImageStorageService imageStoreageService = new ImageStorageService();

        public List<Photo> GetPhotos()
        {
            var phList = db.Photos.SqlQuery("SELECT * FROM dbo.Photo ORDER BY DateTaken DESC").ToList();

            return phList;
        }

        public Photo GetPhoto(int? photoId)
        {
            if (photoId == null)
            {
                throw new ArgumentException("Photo id not supplied");
            }

            Photo Photo = db.Photos.Find(photoId);
            if (Photo == null)
            {
                throw new ArgumentException("Photo not found for id:" + photoId);
            }

            return Photo;

        }

        public void AddPhoto(Photo ph, string fileNamePrefix, HttpPostedFileBase file)
        {
            var newPhoto = new Photo();

            newPhoto.Description = ph.Description;

            newPhoto.UploadFileName = String.IsNullOrEmpty(fileNamePrefix) ? file.FileName : fileNamePrefix + "_" + file.FileName;
            newPhoto.FileName = newPhoto.UploadFileName;

            DateTime dtSALDBMin = new DateTime(1753, 1, 1);
            newPhoto.DateTaken = ph.DateTaken > dtSALDBMin ? ph.DateTaken : DateTime.Now;

            newPhoto.DateUploaded = DateTime.Now;
            newPhoto.UploadedBy = "BRAD";

            newPhoto.TakenBy = String.IsNullOrEmpty(newPhoto.TakenBy) ?Environment.UserName: newPhoto.TakenBy;

            string storeModeCloud = System.Web.Configuration.WebConfigurationManager.AppSettings["StorageModeCloud"];

            imageStoreageService.SaveImage(file, fileNamePrefix, newPhoto);

            // Save record to database
            db.Photos.Add(newPhoto);
            db.SaveChanges();
        }

        public void DeletePhoto(int? photoId)
        {
            Photo ph = GetPhoto(photoId);

            imageStoreageService.DeleteImage(ph);

            db.Photos.Remove(ph);
            db.SaveChanges();
        }

    }
}