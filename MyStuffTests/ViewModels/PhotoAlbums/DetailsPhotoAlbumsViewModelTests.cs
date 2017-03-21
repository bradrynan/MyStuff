using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyStuff.DAL;
using MyStuff.Models;
using MyStuff.ViewModels.PhotoAlbums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStuff.ViewModels.PhotoAlbums.Tests
{
    [TestClass()]
    public class DetailsPhotoAlbumsViewModelTests
    {
        private GalleryContext db = new GalleryContext();

        [TestMethod()]
        public void DetailsPhotoAlbumsViewModelTest()
        {
            var phAlbum = db.PhotoAlbums.Include("Photos").FirstOrDefault();

            if (phAlbum != null)
            {
                GalleryPhotoAlbumsViewModel vm = new GalleryPhotoAlbumsViewModel(phAlbum.AlbumId);

                Assert.IsNotNull(vm.PhotoAlbum);
                Assert.IsTrue(vm.PhotoAlbum.AlbumId == phAlbum.AlbumId);

                Assert.IsNotNull(vm.PhotoAlbum.Photos);
                Assert.IsTrue(vm.PhotoAlbum.Photos.Count == phAlbum.Photos.Count);
            }
        }
    }
}