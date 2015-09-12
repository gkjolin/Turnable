using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Turnable.Tiled;
using Tests.Factories;

namespace Tests.Tmx
{
    [TestFixture]
    public class PropertyDictionaryTests
    {
        [Test]
        public void DefaultConstructor_Exists()
        {
            PropertyDictionary properties = new PropertyDictionary();

            Assert.AreEqual(0, properties.Count);
        }

        [Test]
        public void Constructor_LoadsUpAllProperties()
        {
            PropertyDictionary properties = new PropertyDictionary(TiledFactory.BuildPropertiesXElements());

            Assert.AreEqual(1, properties.Count);
            Assert.AreEqual("true", properties["IsBackgroundLayer"]);
        }

        [Test]
        public void Indexer_WhenGettingAValue_IgnoresCaseOfPropertyName()
        {
            PropertyDictionary properties = new PropertyDictionary(TiledFactory.BuildPropertiesXElements());

            Assert.AreEqual("true", properties["isbackgroundlayer"]);
        }

        [Test]
        public void Indexer_WhenPropertyDoesNotExist_ReturnsNull()
        {
            PropertyDictionary properties = new PropertyDictionary(TiledFactory.BuildPropertiesXElements());

            Assert.IsNull(properties["DoesNotExist"]);
        }

        [Test]
        public void Indexer_SetsProperty()
        {
            PropertyDictionary properties = new PropertyDictionary(TiledFactory.BuildPropertiesXElements());
            properties["New Property"] = "New Value";

            Assert.AreEqual("New Value", properties["New Property"]);
        }

        [Test]
        public void Indexer_UpdatingPreexistingProperty_SetsNewValue()
        {
            PropertyDictionary properties = new PropertyDictionary(TiledFactory.BuildPropertiesXElements());
            properties["New Property"] = "New Value";
            properties["New Property"] = "New Value 2";

            Assert.AreEqual("New Value 2", properties["New Property"]);
        }

        [Test]
        public void Indexer_UpdatingPreexistingProperty_IgnoresCaseOfPropertyName()
        {
            PropertyDictionary properties = new PropertyDictionary(TiledFactory.BuildPropertiesXElements());
            properties["New Property"] = "New Value";
            properties["new property"] = "New Value 2";

            Assert.AreEqual("New Value 2", properties["New Property"]);
        }
    }
}
