using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyStuff.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStuff.Service.Tests
{
    [TestClass()]
    public class ImageStorageServiceTests
    {
        [TestMethod()]
        public void DeleteAllBlobsTest()
        {
            ImageStorageService iss = new ImageStorageService();

            // iss.DeleteAllBlobs();
        }
    }
}