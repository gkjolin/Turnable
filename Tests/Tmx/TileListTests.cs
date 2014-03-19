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

        [TestMethod]
        public void TileList_ConstructionOfAnEmptyTileList_IsSuccessful()
        {
            TileList tileList = new TileList();

            Assert.AreEqual(0, tileList.Count);
        }

        [TestMethod]
        public void TileList_MergingAnEmptyTileListWithAnotherEmptyTileList_ResultsInAnEmptyTileList()
        {
            TileList tileList1 = new TileList();
            TileList tileList2 = new TileList();

            tileList1.Merge(tileList2);

            Assert.AreEqual(0, tileList1.Count);
        }

        [TestMethod]
        public void TileList_MergingATileListWithAnEmptyTileList_ResultsInTheOriginalTileList()
        {
            TileList tileList1 = new TileList();
            tileList1.Add(new Tuple<int,int>(1, 1), new Tile(1, 1, 1));
            TileList tileList2 = new TileList();

            tileList1.Merge(tileList2);

            Assert.AreEqual(1, tileList1.Count);
        }

        [TestMethod]
        public void TileList_MergingAnEmptyTileListWithAnotherTileList_MergesCorrectly()
        {
            TileList tileList1 = new TileList();
            TileList tileList2 = new TileList();
            tileList2.Add(new Tuple<int, int>(1, 1), new Tile(1, 1, 1));

            tileList1.Merge(tileList2);

            Assert.AreEqual(1, tileList1.Count);
        }

        [TestMethod]
        public void TileList_MergingATileListWithAnotherTileList_MergesCorrectly()
        {
            TileList tileList1 = new TileList();
            tileList1.Add(new Tuple<int, int>(5, 4), new Tile(6, 5, 4));
            TileList tileList2 = new TileList();
            tileList2.Add(new Tuple<int, int>(1, 1), new Tile(1, 1, 1));

            tileList1.Merge(tileList2);

            Assert.AreEqual(2, tileList1.Count);
            Assert.IsTrue(tileList1.ContainsKey(new Tuple<int,int>(5, 4)));
            Assert.IsTrue(tileList1.ContainsKey(new Tuple<int,int>(1, 1)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        // TODO: Return a human-friendly message for the exception in this case
        public void TileList_MergingATileListWithAnotherTileListWhichHasDuplicateKeys_Fails()
        {
            TileList tileList1 = new TileList();
            tileList1.Add(new Tuple<int, int>(1, 1), new Tile(6, 1, 1));
            TileList tileList2 = new TileList();
            tileList2.Add(new Tuple<int, int>(1, 1), new Tile(1, 1, 1));

            tileList1.Merge(tileList2);
        }
    }
}
