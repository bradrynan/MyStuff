using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyStuff.DAL;
using MyStuff.Models;
using System.Drawing;
using System.IO;
using MyStuff.ViewModels.Photos;

namespace MyStuff.Controllers
{
    public class PhotosController : Controller
    {
        private GalleryContext db = new GalleryContext();

        public ActionResult Index(string filter = null, int page = 1, int pageSize = 8)
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

        #region Update Photos
        // Based on Index but will allow updates
        public ActionResult Update(string filter = null, int page = 1, int pageSize = 10)
        {
            UpdatePhotosViewModel vm = new UpdatePhotosViewModel();
            vm.LoadImages(filter, page, pageSize);

            return View(vm);
        }

        // POST: Photos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(List<Photo> Photos, int currentPage, bool deleteFiles = false)
        {
            if (ModelState.IsValid)
            {
                UpdatePhotosViewModel vm = new UpdatePhotosViewModel();

                if (!deleteFiles)
                {
                    vm.UpdateImages(Photos);
                }
                else
                {
                    vm.DeleteImages(Photos);
                    currentPage = 1;
                }
            }

            return RedirectToAction("Update", new { page = currentPage });
        }

        // Show the gallery
        public ActionResult Gallery(string filter = null, int page = 1, int pageSize = 44)
        {
            GalleryPhotosViewModel vm = new GalleryPhotosViewModel(filter, page, pageSize);

            return View(vm);
        }
        #endregion

        // GET: Photos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // GET: Photos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhotoId,UploadFileName,FileName,Description,ImagePath,CreatedOn,TakenBy")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(photo);
        }

        #region Delete
        // GET: Photos/Delete/5
        public ActionResult Delete(int? photoId)
        {
            return View(new DeletePhotosViewModel(photoId));
        }


        // POST: Photos/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DeletePhotosViewModel vm)
        {
            new DeletePhotosViewModel().DeletePhoto(vm.Photo.PhotoId);

            return RedirectToAction("Update");
        }
        #endregion Delete


        #region Upload Files
        // Upload
        //[Authorize]
        [HttpGet]
        public ActionResult Upload()
        {
            UploadPhotosViewModel vm = new UploadPhotosViewModel(User.Identity.Name);

            return View(vm);
        }

        //[HttpPost, Authorize]
        [HttpPost]
        public ActionResult UploadFiles()
        {
            UploadPhotosViewModel viewModel = new UploadPhotosViewModel(User.Identity.Name);

            // Checking no of files injected in Request object
            if (Request.Files.Count > 0)
            {
                try
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i];
                        var lastModifiedDate = Request.Form.Get(file.FileName);
                        var photoDescription = Request.Form.Get("photoDescription");
                        var userName = User.Identity.Name;
                        var photoTakenBy = Request.Form.Get("photoTakenBy");
                        viewModel.UploadFile(file, lastModifiedDate, photoDescription, photoTakenBy, userName);
                    }

                    // Returns message that successfully uploaded
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        [HttpPost]
        public ActionResult Upload(UploadPhotosViewModel viewModel, IEnumerable<HttpPostedFileBase> files)
        {
            //if (!ModelState.IsValid)
            //{
            //    viewModel.ErrorMessage = "Model is invalid.";
            //    return View(viewModel);
            //}

            //viewModel.UploadFiles(files);

            //if (!String.IsNullOrEmpty(viewModel.ErrorMessage))
            //{
            //    return View(viewModel);
            //}

            return RedirectPermanent("/photos/gallery");
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



    }
}
