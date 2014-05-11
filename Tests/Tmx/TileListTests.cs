using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Tmx;
using Tests.Factories;
using System.Tuples;
using TurnItUp.Locations;
using TurnItUp.Interfaces;
using Entropy;

namespace Tests.Tmx
{
    [TestClass]
    public class TileListTests
    {
        private ILevel _level;
        private IWorld _world;

        [TestInitialize]
        public void Initialize()
        {
            _world = new World();
            _level = new Level(_world);
            _level.SetUpMap("../../Fixtures/FullExample.tmx");
        }

        [TestMethod]
        public void TileList_ConstructionUsingDataWithNoTiles_IsSuccessful()
        {
            _world = new World();
            _level = new Level(_world);
            _level.SetUpMap("../../Fixtures/MinimalBase64Zlib.tmx");
            TileList tileList = new TileList(_level.Map.Layers[0], TmxFactory.BuildDataWithNoTiles());

            Assert.AreEqual(0, tileList.Count);
        }

        [TestMethod]
        public void TileList_ConstructionUsingDataWithTiles_IsSuccessful()
        {
            TileList tileList = new TileList(_level.Map.Layers["Characters"], TmxFactory.BuildDataWithTiles());

            Assert.AreEqual(8, tileList.Count);

            // Just test to see if one tile is loaded up correctly, in this case the Player (Gid = 2107) located at Tiled location 7,14
            // However we "flip" the Y co-ordinate using Map.Height - Y - 1, 
            // since most libraries have the origin located at the bottom left and Y increasing going North (this is more traditional)
            // rather than the origin located at the top left and Y increasing going South (which is how .tmx does it)
            Assert.AreEqual((uint)2397, tileList[new Tuple<int, int>(8, 7)].Gid);
            Assert.AreEqual(8, tileList[new Tuple<int, int>(8, 7)].X);
            Assert.AreEqual(7, tileList[new Tuple<int, int>(8, 7)].Y);
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
