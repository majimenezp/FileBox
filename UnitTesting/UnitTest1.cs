using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileBox;

namespace UnitTesting
{
    [TestClass]
    public class Methods_Test
    {
        [TestMethod]
        public void Url_generation()
        {
            string id1=UrlGenerator.CreateShortUrlFromId(1001414134242424);

            Assert.IsTrue(!string.IsNullOrEmpty(id1));
        }
    }
}
