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
        public void SetUp()
        {
            _elementList = new ElementList<Layer>();
        }

        [Test]
        public void Add_AddsElementThatCanBeReferencedByIndexOrName()
        {
            Layer layer = TiledFactory.BuildLayer();

            _elementList.Add(layer);

            Assert.That(_elementList[0], Is.SameAs(layer));
            Assert.That(_elementList[layer.Name], Is.SameAs(layer));
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
                Assert.That(_elementList[i], Is.SameAs(layers[i]));
            }
        }

        [Test]
        public void Add_WhenAnElementWithTheSameNameAlreadyExists_ThrowsAnException()
        {
            Layer layer = TiledFactory.BuildLayer();

            _elementList.Add(layer);
            Assert.That(() => _elementList.Add(layer), Throws.ArgumentException);
        }
    }
}
