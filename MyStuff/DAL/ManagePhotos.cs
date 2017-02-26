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

        public void AddPhoto(Photo ph, string fileNamePrefix, HttpPostedFileBase file)
        {
            var newPhoto = new Photo();

            newPhoto.Description = ph.Description;

            newPhoto.UploadFileName = String.IsNullOrEmpty(fileNamePrefix) ? file.FileName : fileNamePrefix + "_" + file.FileName;
            newPhoto.FileName = newPhoto.UploadFileName;

            DateTime dtSALDBMin = new DateTime(1753, 1, 1);
            newPhoto.CreatedOn = ph.CreatedOn > dtSALDBMin ? ph.CreatedOn : DateTime.Now;

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

        public void DeletePhoto(Photo ph)
        {

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
            }
        }


        private void SaveImageCloud(HttpPostedFileBase file, Photo photo)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("managemystuffphotos");

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.FileName);

            photo.ImagePath = blockBlob.Uri.ToString();

            // Create or overwrite the "myblob" blob with contents from a local file.
            using (var fileStream = file.InputStream)
            {
                blockBlob.UploadFromStream(fileStream);
            }

            // blockBlob.DownloadToStream(file.InputStream);
        }       
    }
}