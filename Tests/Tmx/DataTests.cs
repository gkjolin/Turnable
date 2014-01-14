using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Tmx;
using System.IO;

namespace Tests.Tmx
{
    [TestClass]
    public class DataTests
    {
        private XDocument _xDocument;
        private XElement _xMap;
        private XElement _xLayer;
        private XElement _xData;

        private void Load(string path)
        {
            _xDocument = XDocument.Load(path);
            _xMap = _xDocument.Element("map");
            _xLayer = _xMap.Elements("layer").First<XElement>();
            _xData = _xLayer.Element("data");
        }

        [TestMethod]
        public void Data_ConstructionWithZlibCompressedData_IsSuccessful()
        {
            Load("../../Fixtures/MinimalBase64Zlib.tmx");

            Data data = new Data(_xData);

            using (BinaryReader reader = new BinaryReader(data.Contents))
            {
                Assert.AreEqual((UInt32)0, reader.ReadUInt32());
            }
        }

        [TestMethod]
        public void Data_ConstructionWithGzipCompressedData_IsSuccessful()
        {
            Load("../../Fixtures/MinimalBase64Gzip.tmx");

            Data data = new Data(_xData);

            using (BinaryReader reader = new BinaryReader(data.Contents))
            {
                Assert.AreEqual((UInt32)0, reader.ReadUInt32());
            }
        }
    }
}
