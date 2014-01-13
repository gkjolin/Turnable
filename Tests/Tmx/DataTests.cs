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

            Data data = new Data(xData);

            Assert.AreEqual(15 * 15 * 4, data.Contents.length);
        }
    }
}
