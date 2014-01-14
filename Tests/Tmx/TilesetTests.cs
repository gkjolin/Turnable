using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Tmx;
using Tests.Factories;

namespace Tests.Tmx
{
    [TestClass]
    public class TilesetTests
    {
        [TestMethod]
        public void Tileset_Construction_IsSuccessful()
        {
            Tileset tileset = new Tileset(TmxFactory.BuildTilesetXElement());

            Assert.AreEqual(1, tileset.FirstGid);
            Assert.AreEqual("World", tileset.Name);
            Assert.AreEqual(24, tileset.TileWidth);
            Assert.AreEqual(24, tileset.TileHeight);
            Assert.AreEqual(0, tileset.Spacing);
            Assert.AreEqual(0, tileset.Margin);
        }
    }
}
