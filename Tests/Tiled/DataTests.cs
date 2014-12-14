using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Tiled;
using System.IO;

namespace Tests.Tiled
{
    [TestClass]
    public class DataTests
    {
        private XElement _data;

        private void Load(string tmxFullFilePath)
        {
            _data = XDocument.Load(tmxFullFilePath).Element("map").Elements("layer").First<XElement>().Element("data");
        }

        [TestMethod]
        public void Constructor_GivenAPathToAMinimalBase64GzipCompressedTmxFile_SuccessfullyDecodesTheDataInTheFile()
        {
            Load("../../Fixtures/MinimalBase64GzipCompressed.tmx");

            Data data = new Data(_data);

            using (BinaryReader reader = new BinaryReader(data.Contents))
            {
                Assert.AreEqual((UInt32)0, reader.ReadUInt32());
            }
        }

        [TestMethod]
        public void Constructor_GivenAPathToAMinimalBase64ZlibCompressedTmxFile_SuccessfullyDecodesTheDataInTheFile()
        {
            Load("../../Fixtures/MinimalBase64ZlibCompressed.tmx");

            Data data = new Data(_data);

            using (BinaryReader reader = new BinaryReader(data.Contents))
            {
                Assert.AreEqual((UInt32)0, reader.ReadUInt32());
            }
        }
    }
}
