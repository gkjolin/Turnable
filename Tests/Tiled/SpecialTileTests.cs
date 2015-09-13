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

            Assert.That(specialTile.Id, Is.EqualTo((uint)1));
            Assert.That(specialTile.Tileset, Is.SameAs(_tileset));
        }

        [Test]
        public void Constructor_GivenSpecialTileDataWithProperies_InitializesAllPropertiesCorrectly()
        {
            SpecialTile specialTile = new SpecialTile(_tileset, TiledFactory.BuildSpecialTileXElementWithProperties());

            Assert.That(specialTile.Id, Is.EqualTo(332));
            Assert.That(specialTile.Tileset, Is.SameAs(_tileset));
            // Have the properties been loaded?
            Assert.That(specialTile.Properties, Is.Not.Null);
            Assert.That(specialTile.Properties.Count, Is.EqualTo(1));
            Assert.That(specialTile.Properties["Property"], Is.EqualTo("Value"));
        }

        [Test]
        public void GlobalId_IsCalculatedFromTheId()
        {
            SpecialTile specialTile = new SpecialTile(_tileset, 1);

            Assert.That(specialTile.GlobalId, Is.EqualTo(specialTile.Id + _tileset.FirstGlobalId));
        }
    }
}
