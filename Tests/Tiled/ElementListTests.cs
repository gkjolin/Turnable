using System;
using NUnit.Framework;
using Tests.Factories;
using Turnable.Tiled;

namespace Tests.Tiled
{
    [TestFixture]
    public class ElementListTests
    {
        private ElementList<Layer> _elementList;

        [SetUp]
        public void Initialize()
        {
            _elementList = new ElementList<Layer>();
        }

        [Test]
        public void Add_AddsElementThatCanBeReferencedByIndexOrName()
        {
            Layer layer = TiledFactory.BuildLayer();

            _elementList.Add(layer);

            Assert.That(layer, _elementList[0]);
            Assert.That(layer, _elementList[layer.Name]);
        }

        [Test]
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
                Assert.That(layers[i], _elementList[i]);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_WhenAnElementWithTheSameNameAlreadyExists_ThrowsAnException()
        {
            Layer layer = TiledFactory.BuildLayer();

            _elementList.Add(layer);
            _elementList.Add(layer);
        }
    }
}
