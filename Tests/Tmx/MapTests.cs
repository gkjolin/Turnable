using System;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Tmx;

namespace Tests.Tmx
{
    [TestClass]
    public class MapTests
    {
        [TestMethod]
        public void Map_ConstructionUsingMinimalTmxFile_IsSuccessful()
        {
            Map map = new Map("../../Fixtures/Minimal.tmx");

            Assert.AreEqual("1.0", map.Version);
            Assert.AreEqual(Orientation.Orthogonal, map.Orientation);
            Assert.AreEqual(15, map.Width);
            Assert.AreEqual(15, map.Height);
            Assert.AreEqual(24, map.TileWidth);
            Assert.AreEqual(24, map.TileHeight);
        }
    }
}
