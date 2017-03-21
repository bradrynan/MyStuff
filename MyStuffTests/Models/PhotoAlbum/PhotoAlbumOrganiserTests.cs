using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyStuff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStuff.Models.PhotoAlbum.Tests
{
    [TestClass()]
    public class PhotoAlbumOrganiserTests
    {
        [TestMethod()]
        public void CreateAlbumsByYearTest()
        {
            PhotoAlbumOrganiser pao = new PhotoAlbumOrganiser();

            pao.CreateAlbumsByYear(null);
        }
    }
}