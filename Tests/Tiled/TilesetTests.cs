using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Factories;
using Turnable.Tiled;

namespace Tests.Tiled
{
    [TestClass]
    public class TilesetTests
    {
        [TestMethod]
        public void Constructor_GivenATilesetXElement_SuccessfullyInitializesAllProperties()
        {
            Tileset tileset = new Tileset(TiledFactory.BuildTilesetXElement());

            Assert.AreEqual((uint)1, tileset.FirstGlobalId);
            Assert.AreEqual("World", tileset.Name);
            Assert.AreEqual(24, tileset.TileWidth);
            Assert.AreEqual(24, tileset.TileHeight);
            Assert.AreEqual(0, tileset.Spacing);
            Assert.AreEqual(0, tileset.Margin);
        }
    }
}
