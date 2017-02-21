using MyStuff.DAL;
using MyStuff.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyStuff.Controllers
{
    public class HomeController : Controller
    {
        private GalleryContext db = new GalleryContext();

        public ActionResult Index(string filter = null, int page = 1, int pageSize = 10)
        {
            var records = new PagedList<Photo>();
            ViewBag.filter = filter;

            records.Content = db.Photos
                        .Where(x => filter == null || (x.Description.Contains(filter)))
                        .OrderByDescending(x => x.PhotoId)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            records.TotalRecords = db.Photos
                            .Where(x => filter == null || (x.Description.Contains(filter))).Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;

            return View(records);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var photo = new Photo();
            return View(photo);
        }

        public Size NewImageSize(Size imageSize, Size newSize)
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
                newImg.Save(Server.MapPath(pathToSave), img.RawFormat);
            }
        }

        [HttpPost]
        public ActionResult Create(Photo photo, IEnumerable<HttpPostedFileBase> files)
        {
            if (!ModelState.IsValid)
                return View(photo);
            if (files.Count() == 0 || files.FirstOrDefault() == null)
            {
                ViewBag.error = "Please choose a file";
                return View(photo);
            }

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
                model.CreatedOn = fi.LastWriteTime > dtSALDBMin ? fi.LastWriteTime : dtSALDBMin;
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

            return RedirectPermanent("/home");
        }
    }
}