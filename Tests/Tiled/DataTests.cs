using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Turnable.Tiled;

namespace Tests.Tiled
{
    [TestFixture]
    public class DataTests
    {
        [Test]
        public void Constructor_GivenAPathToAMinimalBase64GzipCompressedTmxFile_DecodesTheDataInTheFile()
        {
            var tmxDocument = XDocument.Load("c:/git/Turnable/Tests/SampleData/TmxMap.tmx");
            var tmxLayerData = from l in tmxDocument.Elements("map").Elements("layer")
                where (l.Attribute("name").Value == "walls")
                select l;

            var data = new Data(tmxLayerData.First().Element("data"));

            using (BinaryReader reader = new BinaryReader(data.Contents))
            {
                Assert.That(reader.ReadUInt32(), Is.EqualTo((UInt32)242));
            }
        }
    }
}