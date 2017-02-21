using MyStuff.DAL;
using MyStuff.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace MyStuff.ViewModels
{
    public class UploadPhotosViewModel
    {
        private GalleryContext db = new GalleryContext();
        public string  ErrorMessage { get; set; }
        public Photo Photo { get; set; }

        public UploadPhotosViewModel()
        {
            Photo = new Photo();


            Photo.CreatedOn = DateTime.Today;
            Photo.Description = "UPLOAD";
        }

        public void UploadFiles(IEnumerable<HttpPostedFileBase> files)
        {
            if (!IsValid(files))
                return;

            UploadFiles(this.Photo, files);
        }

        private void UploadFiles(Photo photo, IEnumerable<HttpPostedFileBase> files)
        {

            var model = new Photo();

            foreach (var file in files)
            {
                if (file.ContentLength == 0) continue;

                model.Description = photo.Description;
                var fileName = Guid.NewGuid().ToString();
                var extension = System.IO.Path.GetExtension(file.FileName).ToLower();
                FileInfo fi = new FileInfo(file.FileName);
                model.UploadFileName = fi.Name;
                model.FileName = fi.Name;
                DateTime dtSALDBMin = new DateTime(1753, 1, 1);
                model.CreatedOn = fi.LastWriteTime > dtSALDBMin ? fi.LastWriteTime : DateTime.Now;
                // model.CreatedOn = fi.LastWriteTime;
                model.TakenBy = Environment.UserName;

                using (var img = System.Drawing.Image.FromStream(file.InputStream))
                {
                    model.ImagePath = String.Format("/GalleryImages/{0}{1}", fileName, extension);

                    // Save large size image, 800 x 800
                    SaveToFolder(img, fileName, extension, new Size(800, 800), model.ImagePath);
                }

                // Save record to database
                db.Photos.Add(model);
                db.SaveChanges();
            }
        }
        private bool IsValid(IEnumerable<HttpPostedFileBase> files)
        {
            if (files.Count() == 0 || files.FirstOrDefault() == null)
            {
                ErrorMessage = "Please choose a file";
                return false;
            }

            return true;
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