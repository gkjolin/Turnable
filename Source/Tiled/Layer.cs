using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using System.Xml.Linq;

namespace Turnable.Tiled
{
    public class Layer
    {
        public string Name { get; private set; }
        public double Opacity { get; private set; }
        public bool IsVisible { get; private set; }
        // The .tmx specification https://github.com/marshallward/TiledSharp specifies that Width and Height are the same as the Width and Height of the map
        // However for convenience, we also set the Width and Height for each Layer of a Map during construction
        public int Width { get; private set; }
        public int Height { get; private set; }
        public TileList Tiles { get; private set; }
        public PropertyDictionary Properties { get; private set; }

        public Layer(XElement xLayer)
        {
            Name = (string)xLayer.Attribute("name");
            Opacity = (double?)xLayer.Attribute("opacity") ?? 1.0;
            IsVisible = (bool?)xLayer.Attribute("visible") ?? true;
            Width = (int)xLayer.Attribute("width");
            Height = (int)xLayer.Attribute("height");

            // Load up the Tiles in this layer
            Data data = new Data(xLayer.Element("data"));
            Tiles = new TileList(Width, Height, data);

            // Load up the Properties for this layer, if it exists
            if (xLayer.Element("properties") != null)
            {
                IEnumerable<XElement> xProperties = xLayer.Element("properties").Elements("property");
                Properties = new PropertyDictionary(xProperties);
            }
        }
    }
}
