using MyStuff.DAL;
using MyStuff.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyStuff.Service
{
    public class ManagePhotoAlbumService
    {
        private GalleryContext db = new GalleryContext();

        public List<PhotoAlbum> GetPhotoAlbums()
        {
            return db.PhotoAlbums.Include("Photos").ToList();
        }

        public PhotoAlbum GetPhotoAlbum(int photoAlbumId)
        {
            var phAlbum = db.PhotoAlbums.Include("Photos")
                .Where(p => p.AlbumId == photoAlbumId).FirstOrDefault();

            return phAlbum;
        }

        public PhotoAlbum GetPhotoAlbum(string photoAlbumName)
        {
            var pa = db.PhotoAlbums.Include("Photos")
                .Where(p => p.Name == photoAlbumName).FirstOrDefault();

            return pa;
        }

        public void AddPhotoAlbum(PhotoAlbum photoAlbum)
        {
            db.PhotoAlbums.Add(photoAlbum);
            Save();
        }

        public void AddPhoto(PhotoAlbum photoAlbum, Photo photo)
        {
            db.Entry(photo).State = EntityState.Unchanged;
            photoAlbum.Photos.Add(photo);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}