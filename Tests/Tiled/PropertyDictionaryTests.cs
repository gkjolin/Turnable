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

            Assert.That(properties.Count, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_LoadsUpAllProperties()
        {
            PropertyDictionary properties = new PropertyDictionary(TiledFactory.BuildPropertiesXElements());

            Assert.That(properties.Count, Is.EqualTo(1));
            Assert.That(properties["IsBackgroundLayer"], Is.EqualTo("true"));
        }

        [Test]
        public void Indexer_WhenGettingAValue_IgnoresCaseOfPropertyName()
        {
            PropertyDictionary properties = new PropertyDictionary(TiledFactory.BuildPropertiesXElements());

            Assert.That(properties["isbackgroundlayer"], Is.EqualTo("true"));
        }

        [Test]
        public void Indexer_WhenPropertyDoesNotExist_ReturnsNull()
        {
            PropertyDictionary properties = new PropertyDictionary(TiledFactory.BuildPropertiesXElements());

            Assert.That(properties["DoesNotExist"], Is.Null);
        }

        [Test]
        public void Indexer_SetsProperty()
        {
            PropertyDictionary properties = new PropertyDictionary(TiledFactory.BuildPropertiesXElements());
            properties["New Property"] = "New Value";

            Assert.That(properties["New Property"], Is.EqualTo("New Value"));
        }

        [Test]
        public void Indexer_UpdatingPreexistingProperty_SetsNewValue()
        {
            PropertyDictionary properties = new PropertyDictionary(TiledFactory.BuildPropertiesXElements());
            properties["New Property"] = "New Value";
            properties["New Property"] = "New Value 2";

            Assert.That(properties["New Property"], Is.EqualTo("New Value 2"));
        }

        [Test]
        public void Indexer_UpdatingPreexistingProperty_IgnoresCaseOfPropertyName()
        {
            PropertyDictionary properties = new PropertyDictionary(TiledFactory.BuildPropertiesXElements());
            properties["New Property"] = "New Value";
            properties["new property"] = "New Value 2";

            Assert.That(properties["New Property"], Is.EqualTo("New Value 2"));
        }
    }
}
