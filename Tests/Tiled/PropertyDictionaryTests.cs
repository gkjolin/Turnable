using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Tiled;
using Tests.Factories;

namespace Tests.Tmx
{
    [TestClass]
    public class PropertyDictionaryTests
    {
        [TestMethod]
        public void Constructor_SuccessfullyLoadsUpAllProperties()
        {
            PropertyDictionary properties = new PropertyDictionary(TmxFactory.BuildPropertiesXElements());

            Assert.AreEqual(1, properties.Count);
            Assert.AreEqual("Value", properties["Property"]);
        }

        [TestMethod]
        public void Indexer_IgnoresCaseOfPropertyName()
        {
            PropertyDictionary properties = new PropertyDictionary(TmxFactory.BuildPropertiesXElements());

            Assert.AreEqual("Value", properties["property"]);
        }
    }
}
