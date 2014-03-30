using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using System.Xml.Linq;
using TurnItUp.Components;

namespace TurnItUp.Tmx
{
    public class Layer : IElement
    {
        public string Name { get; private set; }
        public double Opacity { get; private set; }
        public bool IsVisible { get; private set; }
        public TileList Tiles { get; private set; }
        // The .tmx specification https://github.com/marshallward/TiledSharp specifies that Width and Height are the same as the Width and Height of the map
        // However for convenience, we also copy the Width and Height to each Layer of a TileMap during construction
        public int Width { get; private set; }
        public int Height { get; private set; }
        public PropertyDictionary Properties { get; private set; }

        public Layer(XElement xLayer)
        {
            Name = (string)xLayer.Attribute("name");
            Opacity = (double?)xLayer.Attribute("opacity") ?? 1.0;
            IsVisible = (bool?)xLayer.Attribute("visible") ?? true;
            Height = (int)xLayer.Attribute("height");
            Width = (int)xLayer.Attribute("width");

            // Load up the Tiles in this layer
            Data data = new Data(xLayer.Element("data"));
            Tiles = new TileList(this, data);

            // Load up the Properties for this layer, IF it exists
            if (xLayer.Element("properties") != null)
            {
                IEnumerable<XElement> xProperties = xLayer.Element("properties").Elements("property");
                Properties = new PropertyDictionary(xProperties);
            }
        }

        public void MoveTile(Position currentPosition, Position newPosition)
        {
            if (!Tiles.ContainsKey(new Tuple<int, int>(currentPosition.X, currentPosition.Y)))
            {
                throw new InvalidOperationException(String.Format("<Layer::MoveTile> : there is no tile at {0}.", currentPosition.ToString()));
            }

            if (Tiles.ContainsKey(new Tuple<int, int>(newPosition.X, newPosition.Y)))
            {
                throw new InvalidOperationException(String.Format("<Layer::MoveTile> : cannot move tile from {0} to {1} which is already occupied.", currentPosition.ToString(), newPosition.ToString()));
            }

            Tile oldTile = Tiles[new Tuple<int, int>(currentPosition.X, currentPosition.Y)];
            Tile newTile = new Tile(oldTile.Gid, newPosition.X, newPosition.Y);
            Tiles[new Tuple<int, int>(newPosition.X, newPosition.Y)] = newTile;
            Tiles.Remove(new Tuple<int, int>(currentPosition.X, currentPosition.Y));
        }
    }
}
