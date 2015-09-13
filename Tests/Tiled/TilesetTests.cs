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

            Assert.That(tileset.FirstGlobalId, Is.EqualTo((uint)1));
            Assert.That(tileset.Name, Is.EqualTo("World"));
            Assert.That(tileset.TileWidth, Is.EqualTo(24));
            Assert.That(tileset.TileHeight, Is.EqualTo(24));
            Assert.That(tileset.Spacing, Is.EqualTo(0));
            Assert.That(tileset.Margin, Is.EqualTo(0));

            // Have all the special tiles been loaded?
            Assert.That(tileset.SpecialTiles.Count, Is.EqualTo(1));
            Assert.That(tileset.SpecialTiles[332].Properties.Count, Is.EqualTo(1));
        }

        [Test]
        public void FindSpecialTiles_GivenAPropertyNameAndValue_FindsAllSpecialTilesThatMatch()
        {
            Tileset tileset = new Tileset(TiledFactory.BuildTilesetXElementWithSpecialTiles());

            List<SpecialTile> specialTiles = tileset.FindSpecialTiles("IsPC", "true");

            Assert.That(specialTiles.Count, Is.EqualTo(3));
            foreach (SpecialTile specialTile in specialTiles)
            {
                Assert.That(specialTile.Properties["IsPC"], Is.EqualTo("true"));
            }
        }

        [Test]
        public void FindSpecialTiles_WhenGivenAPropertyNameThatDoesNotExist_ReturnsAnEmptyList()
        {
            Tileset tileset = new Tileset(TiledFactory.BuildTilesetXElementWithSpecialTiles());

            List<SpecialTile> specialTiles = tileset.FindSpecialTiles("UnknownPropertyName", "true");

            Assert.That(specialTiles.Count, Is.EqualTo(0));
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

//            Assert.That((uint)2107, tileset.FirstGid);
//            Assert.That("Characters", tileset.Name);
//            Assert.That(24, tileset.TileWidth);
//            Assert.That(24, tileset.TileHeight);
//            Assert.That(0, tileset.Spacing);
//            Assert.That(0, tileset.Margin);
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
//            Assert.That("Knight M", referenceTile.Properties["Model"]);
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
//            Assert.That(referenceTile.Id, tile.Gid - tileset.FirstGid);
//        }
//    }
//}
