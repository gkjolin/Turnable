using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Tmx;
using Tests.Factories;

namespace Tests.Tmx
{
    [TestClass]
    public class PropertyDictionaryTests
    {
        [TestMethod]
        public void PropertyDictionary_Construction_IsSuccessful()
        {
            PropertyDictionary properties = new PropertyDictionary(TmxFactory.BuildPropertiesXElements());
 
            Assert.AreEqual(1, properties.Count);
            Assert.AreEqual("true", properties["IsCharacters"]);
        }

        [TestMethod]
        public void PropertyDictionary_GivenAPropertyNameThatDiffersByCase_IgnoresCase()
        {
            PropertyDictionary properties = new PropertyDictionary(TmxFactory.BuildPropertiesXElements());

            Assert.AreEqual("true", properties["ischaracters"]);
        }

        [TestMethod]
        public void PropertyDictionary_WhenAskedToRetrieveAProperty_ReturnsValueInLowerCase()
        {
            PropertyDictionary properties = new PropertyDictionary(TmxFactory.BuildPropertiesXElements());

            properties["IsCharacters"] = "TRUE";
            Assert.AreEqual("true", properties["IsCharacters"]);
        }
    }
}
