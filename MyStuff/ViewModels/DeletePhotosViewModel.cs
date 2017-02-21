using MyStuff.DAL;
using MyStuff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStuff.ViewModels
{
    public class DeletePhotosViewModel
    {
        private GalleryContext db = new GalleryContext();
        public string ErrorMessage { get; set; }
        public Photo Photo { get; set; }

        public DeletePhotosViewModel()
        {
        }

        public DeletePhotosViewModel(int? photoId)
        {
            ValidatePhotoID(photoId);

            GetPhoto(photoId);
        }

        public void DeletePhoto(int id)
        {
            GetPhoto(id);
            db.Photos.Remove(Photo);
            db.SaveChanges();
        }

        private void ValidatePhotoID(int? photoId)
        {
            if (photoId == null)
            {
                throw new ArgumentException("Photo id not supplied");
            }
        }

        private void GetPhoto(int? photoId)
        {
            Photo = db.Photos.Find(photoId);
            if (Photo == null)
            {
                throw new ArgumentException("Photo not found for id:" + photoId);
            }

        }
    }
}