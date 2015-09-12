using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using System.IO;
using Tests.Factories;
using Turnable.Tiled;

namespace Tests.Tiled
{
    [TestFixture]
    public class SpecialTileTests
    {
        private Tileset _tileset;

        [SetUp]
        public void Initialize()
        {
            _tileset = TiledFactory.BuildTileset();
        }

        [Test]
        public void Constructor_InitializesAllProperties()
        {
            SpecialTile specialTile = new SpecialTile(_tileset, 1);

            Assert.That((uint)1, specialTile.Id);
            Assert.That(_tileset, specialTile.Tileset);
        }

        [Test]
        public void Constructor_GivenSpecialTileDataWithProperies_InitializesAllPropertiesCorrectly()
        {
            SpecialTile specialTile = new SpecialTile(_tileset, TiledFactory.BuildSpecialTileXElementWithProperties());

            Assert.That((uint)332, specialTile.Id);
            Assert.That(_tileset, specialTile.Tileset);
            // Have the properties been loaded?
            Assert.IsNotNull(specialTile.Properties);
            Assert.That(1, specialTile.Properties.Count);
            Assert.That("Value", specialTile.Properties["Property"]);
        }

        [Test]
        public void GlobalId_IsCalculatedFromTheId()
        {
            SpecialTile specialTile = new SpecialTile(_tileset, 1);

            Assert.That(specialTile.Id + _tileset.FirstGlobalId, specialTile.GlobalId);
        }
    }
}
