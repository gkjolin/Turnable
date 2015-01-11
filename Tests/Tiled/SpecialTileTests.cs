using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Tests.Factories;
using Turnable.Tiled;

namespace Tests.Tiled
{
    [TestClass]
    public class SpecialTileTests
    {
        private Tileset _tileset;

        [TestInitialize]
        public void Initialize()
        {
            _tileset = TiledFactory.BuildTileset();
        }

        [TestMethod]
        public void Constructor_InitializesAllProperties()
        {
            SpecialTile specialTile = new SpecialTile(_tileset, 1);

            Assert.AreEqual((uint)1, specialTile.Id);
            Assert.AreEqual(_tileset, specialTile.Tileset);
        }

        [TestMethod]
        public void Constructor_GivenSpecialTileDataWithProperies_InitializesAllPropertiesCorrectly()
        {
            SpecialTile specialTile = new SpecialTile(_tileset, TiledFactory.BuildSpecialTileXElementWithProperties());

            Assert.AreEqual((uint)332, specialTile.Id);
            Assert.AreEqual(_tileset, specialTile.Tileset);
            // Have the properties been loaded?
            Assert.IsNotNull(specialTile.Properties);
            Assert.AreEqual(1, specialTile.Properties.Count);
            Assert.AreEqual("Value", specialTile.Properties["Property"]);
        }

        [TestMethod]
        public void GlobalId_IsSuccessfullyCalculatedFromTheId()
        {
            SpecialTile specialTile = new SpecialTile(_tileset, 1);

            Assert.AreEqual(specialTile.Id + _tileset.FirstGlobalId, specialTile.GlobalId);
        }
    }
}
