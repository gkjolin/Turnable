using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NUnit.Framework;
using Turnable.Tiled;

namespace Tests.Tiled
{
    [TestFixture]
    public class LayerTests
    {
        [Test]
        public void Constructor_GivenAWidthAndHeight_InitializesLayerCorrectly()
        {
            Layer layer = new Layer("Layer 1");

            Assert.That(layer.Name, Is.EqualTo("Layer 1"));
            Assert.That(layer.Opacity, Is.EqualTo(1.0));
            Assert.That(layer.IsVisible, Is.True);
        }

        [Test]
        public void Constructor_GivenALayerXElement_InitializesLayerCorrectly()
        {
            Layer layer = new Layer(XDocument.Load("c:/git/Turnable/Tests/SampleData/TmxMap.tmx").Element("map").Elements("layer").First<XElement>());

            Assert.That(layer.Name, Is.EqualTo("floor"));
            Assert.That(layer.Opacity, Is.EqualTo(1.0));
            Assert.That(layer.IsVisible, Is.True);

            // Are the tiles in the layer created? 
            // TODO: Need a better assertion than the one used below. Asserting that atleast one tile has been loaded isn't the best way to test that the tiles of a lyare are correct. 
            Assert.That(layer.Tiles.Count, Is.GreaterThan(1));

            /*
                        // Are the Properties for this Layer loaded?
                        Assert.That(layer.Properties.Count, Is.EqualTo(1));
            */
        }
    }
}
