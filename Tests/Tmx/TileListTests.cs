using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Tmx;

namespace Tests.Tmx
{
    [TestClass]
    public class TileListTests
    {
        private Data _data;

        private void Load(string path)
        {
            _data = new Data(XDocument.Load(path).Element("map").Elements("layer").First<XElement>().Element("data"));
        }

        [TestMethod]
        public void TileList_ConstructionUsingDataWithNoTiles_IsSuccessful()
        {
            Load("../../Fixtures/MinimalBase64Zlib.tmx");

            TileList tileList = new TileList(_data, 15, 15);

            Assert.AreEqual(0, tileList.Count);
        }

        [TestMethod]
        public void TileList_GivenARowAndCol_CanReturnTileAtThatLocation()
        {
        }
    }
}
