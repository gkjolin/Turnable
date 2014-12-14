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
        //private Layer _layer;

        //[TestInitialize]
        //public void Initialize()
        //{
        //    _layer = TmxFactory.BuildMap().Layers[1];
        //}

        [TestMethod]
        public void Constructor_GivenALayerXElement_SuccessfullyInitializesAllPropertiesAndLoadsUpTheTilesInTheLayer()
        {
            Layer layer = new Layer(TmxFactory.BuildLayerXElement());

            Assert.AreEqual("Background", layer.Name);
            Assert.AreEqual(15, layer.Width);
            Assert.AreEqual(15, layer.Height);
            Assert.AreEqual(1.0, layer.Opacity);
            Assert.AreEqual(true, layer.IsVisible);

            // Are the tiles in the layer created?
            // Assert.AreEqual(layer.Width * layer.Height, layer.Tiles.Count);
        }
    }
}
