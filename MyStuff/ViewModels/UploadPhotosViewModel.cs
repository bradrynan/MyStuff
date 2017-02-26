﻿using MyStuff.DAL;
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
            ManagePhotos managePhotos = new ManagePhotos();
            foreach (var file in files)
            {
                if (file.ContentLength == 0) continue;

                managePhotos.AddPhoto(photo, file); 
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
    }
}