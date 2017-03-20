using MyStuff.DAL;
using MyStuff.Models;
using MyStuff.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStuff.ViewModels.PhotoAlbums
{
    public class DetailsPhotoAlbumsViewModel
    {
        public PhotoAlbum PhotoAlbum { get; set; }

        public DetailsPhotoAlbumsViewModel(int photoAlbumId)
        {
            PhotoAlbum = new ManagePhotoAlbumService().GetPhotoAlbum(photoAlbumId);
        }

    }
}