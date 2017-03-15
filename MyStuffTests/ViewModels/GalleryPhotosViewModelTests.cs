using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyStuff.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStuff.ViewModels.Tests
{
    [TestClass()]
    public class GalleryPhotosViewModelTests
    {
        [TestMethod()]
        public void GalleryPhotosViewModelTest()
        {
            GalleryPhotosViewModel gvm = new GalleryPhotosViewModel();

            Assert.IsNotNull(gvm.Images);
        }
    }
}