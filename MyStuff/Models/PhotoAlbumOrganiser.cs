using MyStuff.DAL;
using MyStuff.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStuff.Models
{
    public class PhotoAlbumOrganiser
    {
        ManagePhotoAlbumService photoAlbumService = new ManagePhotoAlbumService();
        ManagePhotosService photoService = new ManagePhotosService();

        public void CreateAlbumsByYear()
        {
            var phList = photoService.GetPhotos();

            foreach (Photo ph in phList)
            {
                PhotoAlbum phAlbum = GetPhotoAlbum(ph.DateTaken);

                var existingPhoto = phAlbum.Photos.Where(p => p.PhotoId == ph.PhotoId).SingleOrDefault();

                if (existingPhoto == null)
                {
                    phAlbum.Photos.Add(ph);
                }
            }

            photoAlbumService.Save();
        }

        private PhotoAlbum GetPhotoAlbum(DateTime DateTaken)
        {
            var photoAlbumName = "Taken in:" + DateTaken.Year.ToString();

            var photoAlbum = photoAlbumService.GetPhotoAlbum(photoAlbumName);

            if (photoAlbum == null)
            {
                photoAlbum = new PhotoAlbum {
                    CreatedBy = "BRAD",
                    DateCreated = DateTime.Now,
                    Description = "Taken in year: " + DateTaken.Year.ToString(),
                    Name = photoAlbumName,
                    Photos = new List<Photo>()
                };

                photoAlbumService.AddPhotoAlbum(photoAlbum);
            }

            return photoAlbum;
        }
    }
}