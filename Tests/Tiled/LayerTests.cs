using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Tiled;
using Tests.Factories;
using System.Tuples;

namespace Tests.Tiled
{
    [TestClass]
    public class LayerTests
    {
        [TestMethod]
        public void Constructor_GivenALayerXElement_SuccessfullyInitializesLayerIncludingPropertiesAndTiles()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElement());

            Assert.AreEqual("Background", layer.Name);
            Assert.AreEqual(15, layer.Width);
            Assert.AreEqual(15, layer.Height);
            Assert.AreEqual(1.0, layer.Opacity);
            Assert.AreEqual(true, layer.IsVisible);

            // Are the tiles in the layer created? 
            // In the case of the fixture that we use (FullExample.tmx), the background is fully filled in, so this simple assert is enough.
            // Tiles is a sparse dictionary, so the below Assert will not always be true in most cases.
            Assert.AreEqual(layer.Width * layer.Height, layer.Tiles.Count);

            // Are the Properties for this Layer loaded?
            Assert.AreEqual(1, layer.Properties.Count);
        }
    }
}
