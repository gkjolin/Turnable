using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Factories;
using Turnable.Tiled;

namespace Tests.Tiled
{
    [TestClass]
    public class ElementListTests
    {
        private ElementList<Layer> _elementList;

        [TestInitialize]
        public void Initialize()
        {
            _elementList = new ElementList<Layer>();
        }

        [TestMethod]
        public void Add_AddsElementThatCanBeReferencedByIndexOrName()
        {
            Layer layer = TiledFactory.BuildLayer();

            _elementList.Add(layer);

            Assert.AreEqual(layer, _elementList[0]);
            Assert.AreEqual(layer, _elementList[layer.Name]);
        }

        [TestMethod]
        public void Add_WhenMultipleElementsAreAdded_KeepsTheOrderInWhichTheElementsAreAdded()
        {
            Layer[] layers = new Layer[3];

            for(int i = 0; i < 3; i++)
            {
                layers[i] = TiledFactory.BuildLayer();
                layers[i].Name = String.Format("Layer {0}", i);
                _elementList.Add(layers[i]);
            }

            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(layers[i], _elementList[i]);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_WhenAnElementWithTheSameNameAlreadyExists_ThrowsAnException()
        {
            Layer layer = TiledFactory.BuildLayer();

            _elementList.Add(layer);
            _elementList.Add(layer);
        }
    }
}
