using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MyStuff.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace MyStuff.Service
{
    public class ImageStorageService
    {
        string storeModeCloud = System.Web.Configuration.WebConfigurationManager.AppSettings["StorageModeCloud"];

        public void SaveImage(HttpPostedFileBase file, string fileNamePrefix, Photo photo)
        {
            if (storeModeCloud == "false")
            {
                SaveImageLocal(file, fileNamePrefix, photo);
            }
            else
            {
                SaveImageCloud(file, photo);
            }

        }

        public void DeleteImage(Photo ph)
        {
            if (storeModeCloud == "false")
            {
                DeleteImageLocal(ph);
            }
            else
            {
                DeleteImageCloud(ph);
            }
        }

        public void DeleteAllBlobs()
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               CloudConfigurationManager.GetSetting("AzureStorageConnectionString"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("managemystuffphotos");

            foreach (IListBlobItem item in container.ListBlobs(null, false))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;

                    blob.DeleteIfExists();

                }               
            }
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