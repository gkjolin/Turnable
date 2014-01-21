using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Tmx;
using Tests.Factories;
using System.Tuples;

namespace Tests.Tmx
{
    [TestClass]
    public class TileListTests
    {
        [TestMethod]
        public void TileList_ConstructionUsingDataWithNoTiles_IsSuccessful()
        {
            TileList tileList = new TileList(TmxFactory.BuildDataWithNoTiles(), 15, 15);

            Assert.AreEqual(0, tileList.Count);
        }

        [TestMethod]
        public void TileList_ConstructionUsingDataWithTiles_IsSuccessful()
        {
            TileList tileList = new TileList(TmxFactory.BuildDataWithTiles(), 15, 15);

            Assert.AreEqual(225, tileList.Count);
            Assert.AreEqual((uint)382, tileList[new Tuple<int, int>(4, 5)].Gid);
            Assert.AreEqual(4, tileList[new Tuple<int, int>(4, 5)].X);
            Assert.AreEqual(5, tileList[new Tuple<int, int>(4, 5)].Y);
        }
    }
}
