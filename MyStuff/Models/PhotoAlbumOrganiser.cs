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

        public void CreateAlbumsByYear(int? photoID)
        {
            var phList = photoService.GetPhotos(photoID);

            foreach (Photo ph in phList)
            {
                PhotoAlbum phAlbum = CreatePhotoAlbumIfNotExists(ph.DateTaken);

                var existingPhoto = phAlbum.Photos.Where(p => p.PhotoId == ph.PhotoId).SingleOrDefault();

                if (existingPhoto == null)
                {
                    photoAlbumService.AddPhoto(phAlbum, ph);
                }
            }

            photoAlbumService.Save();
        }

        private PhotoAlbum CreatePhotoAlbumIfNotExists(DateTime DateTaken)
        {
            var photoAlbumName = DateTaken.Year.ToString();

            var photoAlbum = photoAlbumService.GetPhotoAlbum(photoAlbumName);

            if (photoAlbum == null)
            {
                photoAlbum = new PhotoAlbum {
                    CreatedBy = "BRAD",
                    DateCreated = DateTime.Now,
                    Description = "Photos from " + DateTaken.Year.ToString(),
                    Name = photoAlbumName,
                    Photos = new List<Photo>()
                };

                photoAlbumService.AddPhotoAlbum(photoAlbum);
            }

            return photoAlbum;
        }
    }
}