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
        ManagePhotos manPhoto = new ManagePhotos();

        private GalleryContext db = new GalleryContext();
        public string ErrorMessage { get; set; }
        public Photo Photo { get; set; }

        public DeletePhotosViewModel()
        {
        }

        public DeletePhotosViewModel(int? photoId)
        {
            Photo = manPhoto.GetPhoto(photoId);
        }

        public void DeletePhoto(int photoId)
        {
            manPhoto.DeletePhoto(photoId);           
        }
    }
}