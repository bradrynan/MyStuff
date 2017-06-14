using MyStuff.DAL;
using MyStuff.Models;
using MyStuff.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace MyStuff.ViewModels.Photos
{
    public class UploadPhotosViewModel
    {
        private GalleryContext db = new GalleryContext();
        public string  ErrorMessage { get; set; }

        [Display(Name = "File Name Prefix")]
        public string FileNamePrefix { get; set; }

        public Photo Photo { get; set; }

        public UploadPhotosViewModel(string userName)
        {
            Photo = new Photo();

            Photo.DateUploaded = DateTime.Now;
            Photo.Description = "UPLOAD";

            Photo.TakenBy = Environment.UserName;
        }

        public void UploadFile(HttpPostedFileBase file, 
            string lastModifiedDateTicks, 
            string photoDescription, 
            string photoTakenBy,
            string photoUploadedBy)
        {
            if ((file == null) || (file.ContentLength == 0))
            {
                return;
            }

            // Set the last modified date based on milliseconds (ticks) since epoch.
            try
            {
                long ticks = Convert.ToInt64(lastModifiedDateTicks);

                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var lastModifiedDate = epoch.AddMilliseconds(ticks);

                Photo.DateTaken = lastModifiedDate.ToLocalTime();
            }
            catch (Exception)
            {
                Photo.DateTaken = DateTime.Now.ToLocalTime();
            }

            if (photoDescription != null)
            {
                Photo.Description = photoDescription;
            }

            if (photoTakenBy != null)
            {
                Photo.TakenBy = photoTakenBy;
            }

            if (photoUploadedBy != null)
            {
                Photo.UploadedBy = photoUploadedBy;
            }

            ManagePhotosService managePhotos = new ManagePhotosService();

            managePhotos.AddPhoto(Photo, FileNamePrefix, file);
        }
    }
}