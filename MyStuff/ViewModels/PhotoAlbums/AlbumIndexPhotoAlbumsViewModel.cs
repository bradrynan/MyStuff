using MyStuff.Models;
using MyStuff.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStuff.ViewModels.PhotoAlbums
{
    public class AlbumIndexPhotoAlbumsViewModel
    {
        ManagePhotoAlbumService photoAlbumService = new ManagePhotoAlbumService();

        public PhotoAlbum PhotoAlbum { get; }
        public List<PhotoAlbum> PhotoAlbums  { get; }

        public AlbumIndexPhotoAlbumsViewModel()
        {
            PhotoAlbum = new PhotoAlbum();

            PhotoAlbums = photoAlbumService.GetPhotoAlbums();
        }
    }
}