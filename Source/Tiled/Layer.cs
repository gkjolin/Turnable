using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Turnable.Tiled.Api;
using Turnable.Utilities;
using Turnable.Utilities.Api;

namespace Turnable.Tiled
{
    public class Layer : ILayer
    {
        public bool? IsVisible { get; set; }
        public string Name { get; set; }
        public double? Opacity { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }

        private ITileCollection Tiles { get; set; }

        public int TileCount => Tiles.Count;

        public bool IsTilePresent(ICoordinates coordinates)
        {
            return Tiles[coordinates] != null;
        }

        public Tile GetTile(ICoordinates coordinates)
        {
            return Tiles[coordinates];
        }

        public void SetTile(ICoordinates coordinates, uint globalId)
        {
            Tiles[coordinates] = new Tile(globalId);
        }

        public void RemoveTile(ICoordinates coordinates)
        {
            throw new NotImplementedException();
        }

        public void MoveTile(ICoordinates origin, ICoordinates destination)
        {
            if (Tiles[origin] == null || Tiles[destination] != null)
            {
                throw new InvalidOperationException();
            }

            Tiles[destination] = Tiles[origin];
            Tiles.Remove(origin);
        }

        public void SwapTile(ICoordinates firstCoordinates, ICoordinates secondCoordinates)
        {
            throw new NotImplementedException();
        }

        public Layer(string name)
        {
            Name = name;
            Opacity = 1.0d;
            IsVisible = true;
        }

        public Layer(XElement xLayer) : this((string)xLayer.Attribute("name"))
        {
            Opacity = (double?)xLayer.Attribute("opacity") ?? 1.0;
            IsVisible = (bool?)xLayer.Attribute("visible") ?? true;
            Width = (int?)xLayer.Attribute("width");
            Height = (int?)xLayer.Attribute("height");

            // Load up the Tiles in this layer
            Data data = new Data(xLayer.Element("data"));
            Tiles = new TileCollection((int)Width, (int)Height, data);

            /*

            // Load up the Properties for this layer, if it exists
            if (xLayer.Element("properties") != null)
            {
                IEnumerable<XElement> xProperties = xLayer.Element("properties").Elements("property");
                Properties = new PropertyDictionary(xProperties);
            }
            */
        }
    }
}
