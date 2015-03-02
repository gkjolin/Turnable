using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using System.Xml.Linq;
using Turnable.Api;
using Turnable.Components;

namespace Turnable.Tiled
{
    public class Layer : IElement, ILayer
    {
        public string Name { get; set; }
        public double Opacity { get; set; }
        public bool IsVisible { get; set; }
        // The .tmx specification https://github.com/marshallward/TiledSharp specifies that Width and Height are the same as the Width and Height of the map
        // However for convenience, we also set the Width and Height for each Layer of a Map during construction
        public int Width { get; set; }
        public int Height { get; set; }
        public TileList Tiles { get; set; }
        public PropertyDictionary Properties { get; set; }

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
            else
            {
                Properties = new PropertyDictionary();
            }
        }

        public void SetTile(Position position, uint globalId)
        {
            throw new NotImplementedException();
        }

        public void EraseTile(Position position)
        {
            throw new NotImplementedException();
        }

        public void MoveTile(Position currentPosition, Position newPosition)
        {
            throw new NotImplementedException();
        }

        public void SwapTile(Position position1, Position position2)
        {
            throw new NotImplementedException();
        }
    }
}
