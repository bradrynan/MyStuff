using MyStuff.DAL;
using MyStuff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStuff.Service
{
    public class ManagePhotoAlbumService
    {
        private GalleryContext db = new GalleryContext();

        public PhotoAlbum GetPhotoAlbum(int photoAlbumId)
        {
            PhotoAlbum pa = new PhotoAlbum();

            var phAlbum = db.PhotoAlbums.Include("Photos")
                .Where(p => p.AlbumId == photoAlbumId).FirstOrDefault();

            return pa;
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

        public void Save()
        {
            db.SaveChanges();
        }
    }
}