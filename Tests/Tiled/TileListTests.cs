using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Tiled;
using Tests.Factories;
using System.Tuples;
using Turnable.Components;

namespace Tests.Tiled
{
    [TestClass]
    public class TileListTests
    {
        [TestMethod]
        public void Constructor_CreatesAnEmptyTileList()
        {
            TileList tileList = new TileList();

            Assert.AreEqual(0, tileList.Count);
        }

        [TestMethod]
        public void Constructor_GivenNullData_CreatesAnEmptyTileList()
        {
            TileList tileList = new TileList(15, 15, null);

            Assert.AreEqual(0, tileList.Count);
        }

        [TestMethod]
        public void Constructor_GivenDataWithNoTiles_CreatesAnEmptyTileList()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithNoTiles());

            Assert.AreEqual(0, tileList.Count);
        }
        
        [TestMethod]
        public void Constructor_GivenDataWithTiles_CreatesAllTiles()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithTiles());

            Assert.AreEqual(8, tileList.Count);

            // Test to see if one tile is loaded up correctly. 
            // The Tiled(.tmx) format uses an origin that starts at the top left with Y increasing going down
            // However most libraries use an origin that starts at the bottom left with Y increasing going up
            // So we need to test that Y is "flipped" using (height - row - 1)
            Assert.AreEqual((uint)2107, tileList[new Tuple<int, int>(6, 1)].GlobalId);
            Assert.AreEqual(6, tileList[new Tuple<int, int>(6, 1)].X);
            Assert.AreEqual(1, tileList[new Tuple<int, int>(6, 1)].Y);
        }

        [TestMethod]
        public void Indexer_GivenAPositionWithATileAtThatPosition_ReturnsTheTile()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithTiles());

            // Test to see if one tile is loaded up correctly. 
            Tile tile = tileList[new Position(6, 1)];

            Assert.AreEqual((uint)2107, tile.GlobalId);
            Assert.AreEqual(6, tile.X);
            Assert.AreEqual(1, tile.Y);
        }

        [TestMethod]
        public void Indexer_GivenAPositionWithNoTileAtThatPosition_ReturnsNull()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithTiles());

            // Test to see if one tile is loaded up correctly. 
            Tile tile = tileList[new Position(7, 1)];

            Assert.IsNull(tile);
        }
    }
}
