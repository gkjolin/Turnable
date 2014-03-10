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
            Assert.IsNotNull(tileset.ReferenceTiles);
        }

        [TestMethod]
        public void Tileset_ExternalTilesetConstruction_IsSuccessful()
        {
            Tileset tileset = new Tileset(TmxFactory.BuildExternalTilesetXElement(), "../../Fixtures/FullExample.tmx");

            Assert.AreEqual(2107, tileset.FirstGid);
            Assert.AreEqual("Characters", tileset.Name);
            Assert.AreEqual(24, tileset.TileWidth);
            Assert.AreEqual(24, tileset.TileHeight);
            Assert.AreEqual(0, tileset.Spacing);
            Assert.AreEqual(0, tileset.Margin);
            Assert.IsNotNull(tileset.ReferenceTiles);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Tileset_ExternalTilesetConstructionWhenTheTmxFilePathIsNotPassedIn_ThrowsAnException()
        {
            Tileset tileset = new Tileset(TmxFactory.BuildExternalTilesetXElement());
        }

        [TestMethod]
        public void Tileset_ConstructionUsingTilesetDataWithReferenceTiles_IsSuccessful()
        {
            Tileset tileset = new Tileset(TmxFactory.BuildTilesetXElementWithReferenceTiles());

            Assert.AreEqual(2107, tileset.FirstGid);
            Assert.AreEqual("Characters", tileset.Name);
            Assert.AreEqual(24, tileset.TileWidth);
            Assert.AreEqual(24, tileset.TileHeight);
            Assert.AreEqual(0, tileset.Spacing);
            Assert.AreEqual(0, tileset.Margin);

            // Have the reference tiles been loaded with their properties?
            Assert.AreEqual(4, tileset.ReferenceTiles.Count);
            Assert.AreEqual(2, tileset.ReferenceTiles[0].Properties.Count);
        }

        [TestMethod]
        public void Tileset_GivenAPropertyNameAndValue_CanFindAReferenceTile()
        {
            Tileset tileset = TmxFactory.BuildTileset();

            ReferenceTile referenceTile = tileset.FindReferenceTileByProperty("IsPlayer", "true");

            Assert.IsNotNull(referenceTile);
            Assert.AreEqual("true", referenceTile.Properties["IsPlayer"]);
        }

        [TestMethod]
        public void Tileset_GivenAPropertyNameAndValue_ReturnsNullIfReferenceTileWithPropertyCannotBeFound()
        {
            Tileset tileset = TmxFactory.BuildTileset();

            ReferenceTile referenceTile = tileset.FindReferenceTileByProperty("IsPlayer", "false");

            Assert.IsNull(referenceTile);
        }
    }
}
