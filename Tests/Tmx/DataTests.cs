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
        private XDocument _xDocument;
        private XElement _xMap;
        private XElement _xLayer;
        private XElement _xData;

        [TestInitialize]
        public void Initialize()
        {
        _xDocument = XDocument.Load("../../Fixtures/Minimal.tmx");
        _xMap = _xDocument.Element("map");
        _xLayer = _xMap.Elements("layer").First<XElement>();
        _xData = _xLayer.Element("data");
        }

        [TestMethod]
        public void Data_Construction_IsSuccessful()
        {
            Data data = new Data(_xData);

            Assert.AreEqual(15 * 16 * 4, data.Contents.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Data_ConstructionUsingNonBase64EncodedData_IsUnsuccessful()
        {
            _xData.Attribute("encoding").Value = "base32";

            Data data = new Data(_xData);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Data_ConstructionUsingNonGzipCompressedData_IsUnsuccessful()
        {
            _xData.Attribute("compression").Value = "zlib";

            Data data = new Data(_xData);
        }
    }
}
