using MyStuff.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

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

        public void AddPhoto(Photo ph, HttpPostedFileBase file)
        {
            var newPhoto = new Photo();

            newPhoto.Description = ph.Description;
            var fileName = Guid.NewGuid().ToString();
            var extension = System.IO.Path.GetExtension(file.FileName).ToLower();
            FileInfo fi = new FileInfo(file.FileName);
            newPhoto.UploadFileName = fi.Name;
            newPhoto.FileName = fi.Name;
            DateTime dtSALDBMin = new DateTime(1753, 1, 1);
            newPhoto.CreatedOn = fi.LastWriteTime > dtSALDBMin ? fi.LastWriteTime : DateTime.Now;
            // model.CreatedOn = fi.LastWriteTime;
            newPhoto.TakenBy = Environment.UserName;

            using (var img = System.Drawing.Image.FromStream(file.InputStream))
            {
                newPhoto.ImagePath = String.Format("/GalleryImages/{0}{1}", fileName, extension);

                // Save large size image, 800 x 800
                SaveToFolder(img, fileName, extension, new Size(800, 800), newPhoto.ImagePath);
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

        private Size NewImageSize(Size imageSize, Size newSize)
        {
            Size finalSize;
            double tempval;
            if (imageSize.Height > newSize.Height || imageSize.Width > newSize.Width)
            {
                if (imageSize.Height > imageSize.Width)
                    tempval = newSize.Height / (imageSize.Height * 1.0);
                else
                    tempval = newSize.Width / (imageSize.Width * 1.0);

                finalSize = new Size((int)(tempval * imageSize.Width), (int)(tempval * imageSize.Height));
            }
            else
                finalSize = imageSize; // image is already small size

            return finalSize;
        }

        private void SaveToFolder(Image img, string fileName, string extension, Size newSize, string pathToSave)
        {
            // Get new resolution
            Size imgSize = NewImageSize(img.Size, newSize);
            using (System.Drawing.Image newImg = new Bitmap(img, imgSize.Width, imgSize.Height))
            {
                newImg.Save(HostingEnvironment.MapPath(pathToSave), img.RawFormat);
            }
        }
    }


}