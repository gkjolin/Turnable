using System;
using NUnit.Framework;
using Tests.Factories;
using Turnable.Tiled;
using System.Collections.Generic;

namespace Tests.Tiled
{
    [TestFixture]
    public class TilesetTests
    {
        [Test]
        public void Constructor_GivenATilesetXElement_InitializesAllProperties()
        {
            Tileset tileset = new Tileset(TiledFactory.BuildTilesetXElement());

            Assert.AreEqual((uint)1, tileset.FirstGlobalId);
            Assert.AreEqual("World", tileset.Name);
            Assert.AreEqual(24, tileset.TileWidth);
            Assert.AreEqual(24, tileset.TileHeight);
            Assert.AreEqual(0, tileset.Spacing);
            Assert.AreEqual(0, tileset.Margin);

            // Have all the special tiles been loaded?
            Assert.AreEqual(1, tileset.SpecialTiles.Count);
            Assert.AreEqual(1, tileset.SpecialTiles[332].Properties.Count);
        }

        [Test]
        public void FindSpecialTiles_GivenAPropertyNameAndValue_FindsAllSpecialTilesThatMatch()
        {
            Tileset tileset = new Tileset(TiledFactory.BuildTilesetXElementWithSpecialTiles());

            List<SpecialTile> specialTiles = tileset.FindSpecialTiles("IsPC", "true");

            Assert.AreEqual(3, specialTiles.Count);
            foreach (SpecialTile specialTile in specialTiles)
            {
                Assert.AreEqual("true", specialTile.Properties["IsPC"]);
            }
        }

        [Test]
        public void FindSpecialTiles_WhenGivenAPropertyNameThatDoesNotExist_ReturnsAnEmptyList()
        {
            Tileset tileset = new Tileset(TiledFactory.BuildTilesetXElementWithSpecialTiles());

            List<SpecialTile> specialTiles = tileset.FindSpecialTiles("UnknownPropertyName", "true");

            Assert.AreEqual(0, specialTiles.Count);
        }
    }
}

//namespace Tests.Tmx
//{
//    [TestFixture]
//    public class TilesetTests
//    {
//        [Test]
//        public void Tileset_ExternalTilesetConstruction_IsSuccessful()
//        {
//            Tileset tileset = new Tileset(TmxFactory.BuildExternalTilesetXElement(), "../../Fixtures/FullExample.tmx");

//            Assert.AreEqual((uint)2107, tileset.FirstGid);
//            Assert.AreEqual("Characters", tileset.Name);
//            Assert.AreEqual(24, tileset.TileWidth);
//            Assert.AreEqual(24, tileset.TileHeight);
//            Assert.AreEqual(0, tileset.Spacing);
//            Assert.AreEqual(0, tileset.Margin);
//            Assert.IsNotNull(tileset.ReferenceTiles);
//        }

//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void Tileset_ExternalTilesetConstructionWhenTheTmxFilePathIsNotPassedIn_ThrowsAnException()
//        {
//            Tileset tileset = new Tileset(TmxFactory.BuildExternalTilesetXElement());
//        }

//        [Test]
//        public void Tileset_GivenAPropertyNameAndValue_CanFindAReferenceTile()
//        {
//            Tileset tileset = TmxFactory.BuildTileset();

//            ReferenceTile referenceTile = tileset.FindReferenceTileByProperty("Model", "Knight M");

//            Assert.IsNotNull(referenceTile);
//            Assert.AreEqual("Knight M", referenceTile.Properties["Model"]);
//        }

//        [Test]
//        public void Tileset_GivenAPropertyNameAndValue_ReturnsNullIfReferenceTileWithPropertyCannotBeFound()
//        {
//            Tileset tileset = TmxFactory.BuildTileset();

//            ReferenceTile referenceTile = tileset.FindReferenceTileByProperty("Model", "Nothing");

//            Assert.IsNull(referenceTile);
//        }

//        [Test]
//        public void Tileset_GivenATile_CanFindTheReferenceTile()
//        {
//            Tileset tileset = TmxFactory.BuildTileset();
//            Tile tile = new Tile(2107, 0, 0);

//            ReferenceTile referenceTile = tileset.FindReferenceTileByTile(tile);

//            Assert.IsNotNull(referenceTile);
//            Assert.AreEqual(referenceTile.Id, tile.Gid - tileset.FirstGid);
//        }
//    }
//}
