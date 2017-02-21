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
using MyStuff.ViewModels;

namespace MyStuff.Controllers
{
    public class PhotosController : Controller
    {
        private GalleryContext db = new GalleryContext();

        #region Update Photos
        // Based on Index but will allow updates
        public ActionResult Update(string filter = null, int page = 1, int pageSize = 8)
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

        // POST: Photos/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Update(List<Photo> Photos)
        {
            if (ModelState.IsValid)
            {
                foreach (Photo ph in Photos)
                {
                    bool saveChanges = false;

                    Photo existingPhoto = db.Photos.Find(ph.PhotoId);

                    if (existingPhoto != null)
                    {
                        if (existingPhoto.TakenBy != ph.TakenBy)
                        {
                            existingPhoto.TakenBy = ph.TakenBy;
                            saveChanges = true;
                        }

                        if (existingPhoto.CreatedOn != ph.CreatedOn)
                        {
                            existingPhoto.CreatedOn = ph.CreatedOn;
                            saveChanges = true;
                        }

                        if (existingPhoto.Description != ph.Description)
                        {
                            existingPhoto.Description = ph.Description;
                            saveChanges = true;
                        }

                        if (existingPhoto.FileName != ph.FileName)
                        {
                            existingPhoto.FileName = ph.FileName;
                            saveChanges = true;
                        }
                    }

                    if (saveChanges)
                    {
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Update");
        }

        // Show the gallery
        public ActionResult Gallery(string filter = null, int page = 1, int pageSize = 10)
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

        // GET: Photos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PhotoId,UploadFileName,FileName,Description,ImagePath,CreatedOn,TakenBy")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                db.Photos.Add(photo);
                db.SaveChanges();
                return RedirectToAction("Index");
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

        // GET: Photos/Delete/5
        public ActionResult Delete(int? id)
        {
            DeletePhotosViewModel vm = new DeletePhotosViewModel(id);

            return View(vm);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            new DeletePhotosViewModel().DeletePhoto(id);

            return RedirectToAction("Update");
        }


        #region Upload Files
        // Upload
        [HttpGet]
        public ActionResult Upload()
        {
            UploadPhotosViewModel vm = new UploadPhotosViewModel();

            return View(vm);
        }

        [HttpPost]
        public ActionResult Upload(UploadPhotosViewModel viewModel, IEnumerable<HttpPostedFileBase> files)
        {
            if (!ModelState.IsValid)
            {
                viewModel.ErrorMessage = "Model is invalid.";
                return View(viewModel);
            }

            viewModel.UploadFiles(files);

            if (!String.IsNullOrEmpty(viewModel.ErrorMessage))
            {
                return View(viewModel);
            }

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
