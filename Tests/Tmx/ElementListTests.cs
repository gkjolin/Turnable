using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Tmx;
using System.IO;
using Tests.Factories;

namespace Tests.Tmx
{
    [TestClass]
    public class ElementListTests
    {
        [TestMethod]
        public void ElementList_AddingItem_IsSuccessful()
        {
            ElementList<Layer> elementList = new ElementList<Layer>();
            Layer layer = TmxFactory.BuildLayer();

            elementList.Add(layer);

            Assert.AreEqual(layer, elementList[0]);
            Assert.AreEqual(layer, elementList[layer.Name]);
        }
    }
}
