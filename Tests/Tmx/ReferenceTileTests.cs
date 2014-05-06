using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Tmx;
using System.IO;
using Tests.Factories;

namespace Tests.Tmx
{
    [TestClass]
    public class ReferenceTileTests
    {
        private Tileset _tileset;

        [TestInitialize]
        public void Initialize()
        {
            _tileset = TmxFactory.BuildTileset();
        }

        [TestMethod]
        public void ReferenceTile_Construction_IsSuccessful()
        {
            ReferenceTile referenceTile = new ReferenceTile(_tileset, 1);

            Assert.AreEqual((uint)1, referenceTile.Id);
            Assert.AreEqual(_tileset, referenceTile.Tileset);
        }

        [TestMethod]
        public void ReferenceTile_ConstructionUsingReferenceTileDataWithProperties_IsSuccessful()
        {
            ReferenceTile referenceTile = new ReferenceTile(_tileset, TmxFactory.BuildReferenceTileXElementWithProperties());

            Assert.AreEqual((uint)0, referenceTile.Id);
            Assert.AreEqual(_tileset, referenceTile.Tileset);
            // Properties have been loaded?
            Assert.IsNotNull(referenceTile.Properties);
            Assert.AreEqual(2, referenceTile.Properties.Count);
            Assert.AreEqual("Knight M", referenceTile.Properties["Model"]);
        }

        [TestMethod]
        public void ReferenceTile_ConvertingIdToGid_IsSuccessful()
        {
            ReferenceTile referenceTile = new ReferenceTile(_tileset, 1);

            Assert.AreEqual(1 + _tileset.FirstGid, referenceTile.Gid);
        }
    }
}
