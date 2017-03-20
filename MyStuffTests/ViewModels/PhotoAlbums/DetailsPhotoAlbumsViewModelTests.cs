using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        [TestMethod()]
        public void DetailsPhotoAlbumsViewModelTest()
        {
            DetailsPhotoAlbumsViewModel vm = new DetailsPhotoAlbumsViewModel(1);

            Assert.IsNotNull(vm.PhotoAlbum);
            Assert.IsNotNull(vm.PhotoAlbum.Photos);
        }
    }
}