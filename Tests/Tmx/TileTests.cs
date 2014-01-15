using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Tmx;
using System.IO;

namespace Tests.Tmx
{
    [TestClass]
    public class TileTests
    {
        [TestMethod]
        public void Tile_Construction_IsSuccessful()
        {
            Tile tile = new Tile(32, 1, 2);

            Assert.AreEqual((uint)32, tile.Gid);
            Assert.AreEqual(tile.X, 1);
            Assert.AreEqual(tile.Y, 2);
        }
    }
}
