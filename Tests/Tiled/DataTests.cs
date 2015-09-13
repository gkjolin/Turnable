using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Turnable.Tiled;
using System.IO;

namespace Tests.Tiled
{
    [TestFixture]
    public class DataTests
    {
        private XElement _data;

        private void Load(string tmxFullFilePath)
        {
            _data = XDocument.Load(tmxFullFilePath).Element("map").Elements("layer").First<XElement>().Element("data");
        }

        [Test]
        public void Constructor_GivenAPathToAMinimalBase64GzipCompressedTmxFile_DecodesTheDataInTheFile()
        {
            Load("c:/git/Turnable/Tests/Fixtures/MinimalBase64GzipCompressed.tmx");

            Data data = new Data(_data);

            using (BinaryReader reader = new BinaryReader(data.Contents))
            {
                Assert.That(reader.ReadUInt32(), Is.EqualTo((UInt32)0));
            }
        }

        [Test]
        public void Constructor_GivenAPathToAMinimalBase64ZlibCompressedTmxFile_DecodesTheDataInTheFile()
        {
            Load("c:/git/Turnable/Tests/Fixtures/MinimalBase64ZlibCompressed.tmx");

            Data data = new Data(_data);

            using (BinaryReader reader = new BinaryReader(data.Contents))
            {
                Assert.That(reader.ReadUInt32(), Is.EqualTo((UInt32)0));
            }
        }
    }
}
