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
 
            Assert.AreEqual(2, properties.Count);
            Assert.AreEqual("true", properties["IsCollision"]);
            Assert.AreEqual("Value", properties["Name"]);
        }
    }
}
