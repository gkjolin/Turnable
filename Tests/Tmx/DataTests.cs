using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Tmx;

namespace Tests.Tmx
{
    [TestClass]
    public class DataTests
    {
        [TestMethod]
        public void Data_Construction_IsSuccessful()
        {
            XDocument xDocument = XDocument.Load("../../Fixtures/Minimal.tmx");
            XElement xMap = xDocument.Element("map");
            XElement xLayer = xMap.Elements("layer").First<XElement>();
            XElement xData = xLayer.Element("data");

            Layer layer = new Layer(xLayer);

            Assert.AreEqual("Tile Layer 1", layer.Name);
            Assert.AreEqual(1.0, layer.Opacity);
            Assert.AreEqual(true, layer.IsVisible);
        }
    }
}
