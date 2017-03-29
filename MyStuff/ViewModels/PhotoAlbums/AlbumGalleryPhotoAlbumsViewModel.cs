using MyStuff.DAL;
using MyStuff.Models;
using MyStuff.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStuff.ViewModels.PhotoAlbums
{
    public class AlbumGalleryPhotoAlbumsViewModel
    {
        public PhotoAlbum PhotoAlbum { get; set; }

        public PagedList<Photo> Images { get; set; }


        public AlbumGalleryPhotoAlbumsViewModel(int photoAlbumId, int page = 1, int pageSize = 40)
        {
            PhotoAlbum = new ManagePhotoAlbumService().GetPhotoAlbum(photoAlbumId);

            LoadImages(page, pageSize);
        }

        private void LoadImages(int page, int pageSize)
        {
            Images = new PagedList<Photo>();

            Images.Content = PhotoAlbum.Photos
                        .OrderByDescending(x => x.PhotoId)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            Images.TotalRecords = PhotoAlbum.Photos.Count;

            Images.CurrentPage = page;
            Images.PageSize = pageSize;
        }

    }
}