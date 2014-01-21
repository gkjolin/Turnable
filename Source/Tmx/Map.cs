using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TurnItUp.Tmx
{
    public class Map
    {
        public string Version { get; private set; }
        public Orientation Orientation { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public ElementList<Layer> Layers { get; private set; }
        public ElementList<Tileset> Tilesets { get; private set; }

        public Map(string tmxPath)
        {
            XDocument xDocument = XDocument.Load(tmxPath);
            XElement xMap = xDocument.Element("map");

            Version = (string)xMap.Attribute("version");
            Orientation = (Orientation)Enum.Parse(typeof(Orientation), xMap.Attribute("orientation").Value, true);
            Width = (int)xMap.Attribute("width");
            Height = (int)xMap.Attribute("height");
            TileWidth = (int)xMap.Attribute("tilewidth");
            TileHeight = (int)xMap.Attribute("tileheight");

            Layers = new ElementList<Layer>();
            foreach (XElement xLayer in xMap.Elements("layer"))
            {
                Layers.Add(new Layer(xLayer, Width, Height));
            }

            Tilesets = new ElementList<Tileset>();
            foreach (XElement xTileset in xMap.Elements("tileset"))
            {
                Tilesets.Add(new Tileset(xTileset));
            }
        }

        public Layer FindLayerByProperty(string propertyName, string propertyValue)
        {
            foreach (Layer layer in Layers)
            {
                if (layer.Properties != null && layer.Properties.ContainsKey(propertyName) && layer.Properties[propertyName] == propertyValue)
                {
                    return layer;
                }
            }

            return null;
        }
    }
}
