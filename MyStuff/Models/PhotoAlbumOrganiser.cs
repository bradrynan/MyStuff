using MyStuff.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStuff.Models
{
    public class PhotoAlbumOrganiser
    {
        private GalleryContext db = new GalleryContext();

        public void CreateAlbumsByYear()
        {
            //var phList = db.Photos
            //   .OrderByDescending(x => x.DateTaken).ToList();


            var phList = db.Photos.SqlQuery("SELECT * FROM dbo.Photo ORDER BY DateTaken DESC").ToList();

            foreach (Photo ph in phList)
            {
                PhotoAlbum phAlbum = GetPhotoAlbum(ph.DateTaken);

                var existingPhoto = phAlbum.Photos.Where(p => p.PhotoId == ph.PhotoId).SingleOrDefault();

                if (existingPhoto == null)
                {
                    phAlbum.Photos.Add(ph);
                }
            }

            db.SaveChanges();

        }

        private PhotoAlbum GetPhotoAlbum(DateTime DateTaken)
        {
            var photoAlbumName = "Taken in:" + DateTaken.Year.ToString();



            var SQL = @"SELECT * FROM dbo.Album WHERE Name = '" + photoAlbumName + "'";
            var phAlbum = db.PhotoAlbums.Include("Photos")
                .Where(p => p.Name == photoAlbumName).FirstOrDefault();

            if (phAlbum == null)
            {
                phAlbum = new PhotoAlbum {
                    CreatedBy = "BRAD",
                    DateCreated = DateTime.Now,
                    Description = "Taken in year: " + DateTaken.Year.ToString(),
                    Name = photoAlbumName,
                    Photos = new List<Photo>()
                };

                db.PhotoAlbums.Add(phAlbum);
                db.SaveChanges();
            }

            return phAlbum;
        }
    }
}