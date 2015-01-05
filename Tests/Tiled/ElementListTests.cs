using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Tiled
{
    [TestClass]
    public class ElementListTests
    {
        [TestMethod]
        public void Add_SuccessfullyAddsElementToList()
        {
            ElementList<Layer> elementList = new ElementList<Layer>();
            Layer layer = TmxFactory.BuildLayer();

            elementList.Add(layer);

            Assert.AreEqual(layer, elementList[0]);
            Assert.AreEqual(layer, elementList[layer.Name]);
        }
    }
}
