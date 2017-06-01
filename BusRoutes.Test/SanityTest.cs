using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusRoutes.Test
{
    [TestClass]
    public class SanityTest
    {
        [TestMethod]
        public void DoesTwoEqualTwo()
        {
            Assert.AreEqual(2, 2); //Validate our unit tests actually run properly.
        }
    }
}
