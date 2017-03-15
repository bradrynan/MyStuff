using MyStuff.DAL;
using MyStuff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStuff.ViewModels
{
    public class UpdatePhotosViewModel
    {
        private GalleryContext db = new GalleryContext();
        private ManagePhotos managePhotos = new ManagePhotos();

        public PagedList<Photo> Images { get; set; }

        public string Filter { get; set; }

        public UpdatePhotosViewModel()
        {
        }

        public void DeleteImages(List<Photo> Photos)
        {
            if (Photos == null)
            {
                return;
            }

            foreach (Photo ph in Photos)
            {
                managePhotos.DeletePhoto(ph.PhotoId);
            }
        }

        public void UpdateImages(List<Photo> Photos)
        {
            if (Photos == null)
            {
                return;
            }

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

                    if (existingPhoto.DateUploaded != ph.DateUploaded)
                    {
                        existingPhoto.DateUploaded = ph.DateUploaded;
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

        public void LoadImages(string filter = null, int page = 1, int pageSize = 10)
        {
            Filter = filter;
            Images = new PagedList<Photo>();

            Images.Content = db.Photos
                        .Where(x => filter == null || (x.Description.Contains(filter)))
                        .OrderByDescending(x => x.PhotoId)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            Images.TotalRecords = db.Photos
                            .Where(x => filter == null || (x.Description.Contains(filter))).Count();

            Images.CurrentPage = page;
            Images.PageSize = pageSize;
        }
    }
}