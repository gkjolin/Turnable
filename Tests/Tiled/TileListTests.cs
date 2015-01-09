using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Tiled;
using Tests.Factories;
using System.Tuples;

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
        public void Constructor_GivenDataWithNoTiles_SuccessfullyCreatesAnEmptyTileList()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithNoTiles());

            Assert.AreEqual(0, tileList.Count);
        }
        
        [TestMethod]
        public void Constructor_GivenDataWithTiles_SuccessfullyCreatesAllTiles()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithTiles());

            Assert.AreEqual(6, tileList.Count);

            // Test to see if one tile is loaded up correctly. 
            // The Tiled(.tmx) format uses an origin that starts at the top left with Y increasing going down
            // However most libraries use an origin that starts at the bottom left with Y increasing going up
            // So we need to test that Y is "flipped" using (height - row - 1)
            Assert.AreEqual((uint)2107, tileList[new Tuple<int, int>(6, 1)].GlobalId);
            Assert.AreEqual(6, tileList[new Tuple<int, int>(6, 1)].X);
            Assert.AreEqual(1, tileList[new Tuple<int, int>(6, 1)].Y);
        }
    }
}
