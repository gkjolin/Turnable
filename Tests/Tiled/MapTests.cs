using System;
using NUnit.Framework;
using Turnable.Tiled;
using Turnable.Tiled.Api;

namespace Tests.Tiled
{
    [TestFixture]
    public class MapTests
    {
        [Test]
        public void Constructor_SuccessfullyCreatesANewMap()
        {
            IMap map = new Map();

            Assert.That(map.Version, Is.Null);
            Assert.That(map.Orientation, Is.EqualTo(Orientation.Orthogonal));
            Assert.That(map.RenderOrder, Is.EqualTo(RenderOrder.RightDown));
            Assert.That(map.Width, Is.Null);
            Assert.That(map.Height, Is.Null);
            Assert.That(map.TileWidth, Is.Null);
            Assert.That(map.TileHeight, Is.Null);
        }

        [Test]
        public void Constructor_GivenAFullPathToAMissingFile_ThrowsAnException()
        {
            Assert.That(() => new Map("c:/git/Turnable/Tests/SampleData/Missing.tmx"), Throws.ArgumentException);
        }

        [Test]
        public void Constructor_GivenAPathToATmxFile_CorrectlyInitializesAllProperties()
        {
            Map map = new Map("c:/git/Turnable/Tests/SampleData/TmxMap.tmx");

            Assert.That(map.TileWidth, Is.EqualTo(48));
            Assert.That(map.TileHeight, Is.EqualTo(48));
            Assert.That(map.Width, Is.EqualTo(48));
            Assert.That(map.Height, Is.EqualTo(48));
            Assert.That(map.RenderOrder, Is.EqualTo(RenderOrder.RightDown));
            Assert.That(map.Orientation, Is.EqualTo(Orientation.Orthogonal));
            Assert.That(map.Version, Is.EqualTo("1.0"));

/*
            // Make sure that the layers are loaded up. We really only need to test that all the layers have been initialized with the correct constructor. The actual unit tests for the layer construction in LayerTests is where all the heavy lifting is done. However there is no way (that I know of) to use Moq to verify that the constructor was correctly called for the layer.
            Assert.That(map.Layers.Count, Is.EqualTo(4));
            Assert.That(map.Layers, Is.InstanceOf<ElementList<Layer>>());
            Assert.That(map.Layers[0].Tiles.Count, Is.Not.EqualTo(0));
            Assert.That(map.Layers[0].Properties.Count, Is.EqualTo(1));

            // Tilesets loaded?
            Assert.That(map.Tilesets.Count, Is.EqualTo(2));

            // Is a Bounds for the Map initialized?
            Assert.That(map.Bounds, Is.Not.Null);
            Assert.That(map.Bounds.BottomLeft, Is.EqualTo(new Position(0, 0)));
            Assert.That(map.Bounds.TopRight, Is.EqualTo(new Position(14, 14)));
*/
        }

    }
}
