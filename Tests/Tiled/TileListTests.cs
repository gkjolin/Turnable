using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Turnable.Tiled;
using Tests.Factories;
using Turnable.Components;

namespace Tests.Tiled
{
    [TestFixture]
    public class TileListTests
    {
        [Test]
        public void Constructor_CreatesAnEmptyTileList()
        {
            TileList tileList = new TileList();

            Assert.That(0, tileList.Count);
        }

        [Test]
        public void Constructor_GivenNullData_CreatesAnEmptyTileList()
        {
            TileList tileList = new TileList(15, 15, null);

            Assert.That(0, tileList.Count);
        }

        [Test]
        public void Constructor_GivenDataWithNoTiles_CreatesAnEmptyTileList()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithNoTiles());

            Assert.That(0, tileList.Count);
        }
        
        [Test]
        public void Constructor_GivenDataWithTiles_CreatesAllTiles()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithTiles());

            Assert.That(8, tileList.Count);

            // Test to see if one tile is loaded up correctly. 
            // The Tiled(.tmx) format uses an origin that starts at the top left with Y increasing going down
            // However most libraries use an origin that starts at the bottom left with Y increasing going up
            // So we need to test that Y is "flipped" using (height - row - 1)
            Tile tile = tileList[new Position(6, 1)];
            Assert.That((uint)2107, tile.GlobalId);
            Assert.That(6, tile.X);
            Assert.That(1, tile.Y);
        }

        [Test]
        public void Indexer_GivenAPositionWithATileAtThatPosition_ReturnsTheTile()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithTiles());

            // Test to see if one tile is loaded up correctly. 
            Tile tile = tileList[new Position(6, 1)];

            Assert.That((uint)2107, tile.GlobalId);
            Assert.That(6, tile.X);
            Assert.That(1, tile.Y);
        }

        [Test]
        public void Indexer_GivenAPositionWithNoTileAtThatPosition_ReturnsNull()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithTiles());

            // Test to see if one tile is loaded up correctly. 
            Tile tile = tileList[new Position(7, 1)];

            Assert.IsNull(tile);
        }

        [Test]
        public void Indexer_GivenAPositionAndValue_SetsTileAtThePosition()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithNoTiles());

            Tile tile = new Tile(2107, 6, 1);
            tileList[new Position(6, 1)] = tile;

            tile = tileList[new Position(6, 1)];
            Assert.That((uint)2107, tile.GlobalId);
            Assert.That(6, tile.X);
            Assert.That(1, tile.Y);
        }

        [Test]
        public void Indexer_GivenAPositionThatAlreadyHasATile_OverwritesTilesAtThePosition()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithNoTiles());

            Tile tile = new Tile(2107, 6, 1);
            tileList[new Position(6, 1)] = tile;
            tile = new Tile(2106, 6, 1);
            tileList[new Position(6, 1)] = tile;

            tile = tileList[new Position(6, 1)];
            Assert.That((uint)2106, tile.GlobalId);
            Assert.That(6, tile.X);
            Assert.That(1, tile.Y);
        }

        [Test]
        public void Remove_GivenAPositionThatHasATile_RemovesTileAtThatPosition()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithNoTiles());
            Tile tile = new Tile(2107, 6, 1);
            tileList[new Position(6, 1)] = tile;

            tileList.Remove(new Position(6, 1));

            Assert.IsNull(tileList[new Position(6, 1)]);
        }

        [Test]
        public void Remove_GivenAPositionThatHasNoTile_DoesNotDoAnything()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithNoTiles());

            tileList.Remove(new Position(6, 1));

            Assert.IsNull(tileList[new Position(6, 1)]);
        }
    }
}
