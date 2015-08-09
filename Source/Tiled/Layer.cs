using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using System.Xml.Linq;
using Turnable.Api;
using Turnable.Components;
using Turnable.Utilities;

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
            Tiles[new Tuple<int, int>(position.X, position.Y)] = new Tile((uint)globalId, position.X, position.Y);
        }

        public void RemoveTile(Position position)
        {
            Tiles.Remove(new Tuple<int, int>(position.X, position.Y));
        }

        public void MoveTile(Position currentPosition, Position newPosition)
        {
            if (!Tiles.ContainsKey(new Tuple<int, int>(currentPosition.X, currentPosition.Y)))
            {
                throw new InvalidOperationException();
            }

            if (Tiles.ContainsKey(new Tuple<int, int>(newPosition.X, newPosition.Y)))
            {
                throw new InvalidOperationException();
            }

            Tile existingTile = Tiles[new Tuple<int, int>(currentPosition.X, currentPosition.Y)];
            Tile newTile = new Tile(existingTile.GlobalId, newPosition.X, newPosition.Y);
            SetTile(newPosition, existingTile.GlobalId);
            RemoveTile(currentPosition);
        }

        public void SwapTile(Position position1, Position position2)
        {
            if (!Tiles.ContainsKey(new Tuple<int, int>(position1.X, position1.Y)))
            {
                throw new InvalidOperationException();
            }

            if (!Tiles.ContainsKey(new Tuple<int, int>(position2.X, position2.Y)))
            {
                throw new InvalidOperationException();
            }

            Tile tile1 = Tiles[new Tuple<int, int>(position1.X, position1.Y)];
            Tile tile2 = Tiles[new Tuple<int, int>(position2.X, position2.Y)];
            RemoveTile(position1);
            RemoveTile(position2);
            SetTile(position1, tile2.GlobalId);
            SetTile(position2, tile1.GlobalId);
        }

        public void Fill(uint globalId)
        {
            Fill(new Rectangle(new Position(0, 0), new Position(Width - 1, Height - 1)), globalId);
        }

        public void Fill(Rectangle bounds, uint globalId)
        {
            for (int col = bounds.BottomLeft.X; col <= bounds.TopRight.X; col++)
            {
                for (int row = bounds.BottomLeft.Y; row <= bounds.TopRight.Y; row++)
                {
                    SetTile(new Position(col, row), globalId);
                }
            }
        }
    }
}
