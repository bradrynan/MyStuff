using MyStuff.DAL;
using MyStuff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStuff.ViewModels.Photos
{
    public class GalleryPhotosViewModel
    {
        private GalleryContext db = new GalleryContext();

        public PagedList<Photo> Images { get; set; }

        public string Filter { get; set; }

        public GalleryPhotosViewModel(string filter = null, int page = 1, int pageSize = 10)
        {
            LoadImages(filter, page, pageSize);
        }

        private void LoadImages(string filter = null, int page = 1, int pageSize = 10)
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