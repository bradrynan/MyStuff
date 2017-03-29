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
using MyStuff.ViewModels.PhotoAlbums;

namespace MyStuff.Controllers
{
    public class PhotoAlbumsController : Controller
    {
        private GalleryContext db = new GalleryContext();

        // GET: PhotoAlbums
        public ActionResult AlbumIndex()
        {
            return View(new AlbumIndexPhotoAlbumsViewModel());
        }

        // GET: PhotoAlbums/Gallery/5
        public ActionResult AlbumGallery(int id = -1, int page = 1, int pageSize = 44)
        {
            if (id == -1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AlbumGalleryPhotoAlbumsViewModel vm = new AlbumGalleryPhotoAlbumsViewModel(id, page, pageSize);

            return View(vm);
        }

        // GET: PhotoAlbums/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhotoAlbums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AlbumId,Name,Description,DateCreated,CreatedBy")] PhotoAlbum photoAlbum)
        {
            if (ModelState.IsValid)
            {
                db.PhotoAlbums.Add(photoAlbum);
                db.SaveChanges();
                return RedirectToAction("AlbumIndex");
            }

            return View(photoAlbum);
        }

        // GET: PhotoAlbums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotoAlbum photoAlbum = db.PhotoAlbums.Find(id);
            if (photoAlbum == null)
            {
                return HttpNotFound();
            }
            return View(photoAlbum);
        }

        // POST: PhotoAlbums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AlbumId,Name,Description,DateCreated,CreatedBy")] PhotoAlbum photoAlbum)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photoAlbum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AlbumIndex");
            }
            return View(photoAlbum);
        }

        // GET: PhotoAlbums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotoAlbum photoAlbum = db.PhotoAlbums.Find(id);
            if (photoAlbum == null)
            {
                return HttpNotFound();
            }
            return View(photoAlbum);
        }

        // POST: PhotoAlbums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhotoAlbum photoAlbum = db.PhotoAlbums.Find(id);

            if (photoAlbum != null)
            {
                db.PhotoAlbums.Remove(photoAlbum);
                db.SaveChanges();
            }

            return RedirectToAction("AlbumIndex");
        }

        // GET:
        public ActionResult OrganiseByYear()
        {
            new PhotoAlbumOrganiser().CreateAlbumsByYear(null);

            return View("AlbumIndex",  new AlbumIndexPhotoAlbumsViewModel());
        }

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
