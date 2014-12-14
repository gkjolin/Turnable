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
            TileList tileList = new TileList(15, 15, TmxFactory.BuildDataWithNoTiles());

            Assert.AreEqual(0, tileList.Count);
        }
        
        [TestMethod]
        public void Constructor_GivenDataWithTiles_SuccessfullyCreatesAllTiles()
        {
            TileList tileList = new TileList(15, 15, TmxFactory.BuildDataWithTiles());

            Assert.AreEqual(6, tileList.Count);

            // Test to see if one tile is loaded up correctly.
            Assert.AreEqual((uint)2107, tileList[new Tuple<int, int>(6, 13)].GlobalId);
            Assert.AreEqual(6, tileList[new Tuple<int, int>(6, 13)].X);
            Assert.AreEqual(13, tileList[new Tuple<int, int>(6, 13)].Y);
        }
    }
}
