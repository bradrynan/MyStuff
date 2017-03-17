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

namespace MyStuff.DAL
{
    public class ManagePhotos
    {
        private GalleryContext db = new GalleryContext();



        public string GetFileName(Photo ph)
        {
            string fileName = DateTime.Now.ToString();

            return fileName;
        }

        public void SetFileName(Photo ph)
        {
            string fileName = DateTime.Now.ToString();

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

            if (storeModeCloud == "false")
            {
                SaveImageLocal(file, fileNamePrefix, newPhoto);
            }
            else
            {
                SaveImageCloud(file, newPhoto);
            }

            // Save record to database
            db.Photos.Add(newPhoto);
            db.SaveChanges();
        }

        public void DeletePhoto(int? photoId)
        {
            Photo ph = GetPhoto(photoId);

            string storeModeCloud = System.Web.Configuration.WebConfigurationManager.AppSettings["StorageModeCloud"];

            if (storeModeCloud == "false")
            {
                DeleteImageLocal(ph);
            }
            else
            {
                DeleteImageCloud(ph);
            }

            db.Photos.Remove(ph);
            db.SaveChanges();
        }

        private void DeleteImageLocal(Photo ph)
        {
            string imagePath = HostingEnvironment.MapPath(ph.ImagePath);

            FileInfo fi = new FileInfo(imagePath);
            if (fi.Exists)
            {
                try
                {
                    fi.Delete();
                }
                catch (Exception)
                {
                    // Do nothing - sometimes the images are still being loaded and you get an error.
                    // Just ignore and leave the orphan image on the server.
                    // Maybe write a cleanup job at a later stage.
                }
            }
        }

        private void DeleteImageCloud(Photo ph)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               CloudConfigurationManager.GetSetting("AzureStorageConnectionString"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("managemystuffphotos");

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(ph.UploadFileName);

            if (blockBlob.Exists())
            {
                blockBlob.DeleteIfExists();
            }
        }

        public void UpdatePhoto(Photo ph)
        {

        }

        private void SaveImageLocal(HttpPostedFileBase file, string fileNamePrefix, Photo photo)
        {
            var fileName = String.IsNullOrEmpty(fileNamePrefix) ? Guid.NewGuid().ToString() : fileNamePrefix + "_" + Guid.NewGuid().ToString();
            var extension = System.IO.Path.GetExtension(file.FileName).ToLower();

            using (var img = System.Drawing.Image.FromStream(file.InputStream))
            {
                photo.ImagePath = String.Format("/GalleryImages/{0}{1}", fileName, extension);

                using (System.Drawing.Image newImg = new Bitmap(img, img.Width, img.Height))
                {
                    newImg.Save(HostingEnvironment.MapPath(photo.ImagePath), img.RawFormat);
                }

                photo.ThumbnailPath = String.Format("/GalleryImages/Thumbs/{0}{1}", fileName, extension);

                using (System.Drawing.Image newImg = new Bitmap(img, 100, 100))
                {
                    newImg.Save(HostingEnvironment.MapPath(photo.ThumbnailPath), img.RawFormat);
                }
            }
        }


        private void SaveImageCloud(HttpPostedFileBase file, Photo photo)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               CloudConfigurationManager.GetSetting("AzureStorageConnectionString"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("managemystuffphotos");

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.FileName);

            photo.ImagePath = blockBlob.Uri.ToString();
            photo.ThumbnailPath = blockBlob.Uri.ToString();

            // Create or overwrite the "myblob" blob with contents from a local file.
            using (var fileStream = file.InputStream)
            {
                blockBlob.UploadFromStream(fileStream);
            }

            // blockBlob.DownloadToStream(file.InputStream);
        }
    }
}