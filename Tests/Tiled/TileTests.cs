using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Tiled;
using System.IO;

namespace Tests.Tiled
{
    [TestClass]
    public class TileTests
    {
        [TestMethod]
        public void Constructor_SuccessfullyInitializesAllProperties()
        {
            Tile tile = new Tile(32, 1, 2);

            Assert.AreEqual((uint)32, tile.GlobalId);
            Assert.AreEqual(tile.X, 1);
            Assert.AreEqual(tile.Y, 2);
        }
    }
}
