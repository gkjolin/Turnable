using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Turnable.Api;

namespace Turnable.Tiled
{
    public class Map : IMap
    {
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public RenderOrder RenderOrder { get; set; }
        public Orientation Orientation { get; set; }
        public string Version { get; set; }
        public ElementList<Layer> Layers { get; set; }

        public Map()
        {
        }

        public Map(string tmxFullFilePath)
        {
            XDocument xDocument = XDocument.Load(tmxFullFilePath);
            XElement xMap = xDocument.Element("map");

            TileWidth = (int)xMap.Attribute("tilewidth");
            TileHeight = (int)xMap.Attribute("tileheight");
            Width = (int)xMap.Attribute("width");
            Height = (int)xMap.Attribute("height");
            // TODO: Load up RenderOrder correctly
            Orientation = (Orientation)Enum.Parse(typeof(Orientation), xMap.Attribute("orientation").Value, true);
            Version = (string)xMap.Attribute("version");

            // Load up all the Layers in this Map.
            Layers = new ElementList<Layer>();
            foreach (XElement xLayer in xMap.Elements("layer"))
            {
                Layers.Add(new Layer(xLayer));
            }
        }

        public enum SpecialLayer
        {
            Background,
            Collision,
            Object,
            Character
        }

        public void SetSpecialLayer(Layer layer, SpecialLayer value)
        {
            string key = "Is" + value.ToString() + "Layer";

            if (Layers[0].Properties.ContainsKey(key))
            {
                throw new ArgumentException();
            }
            
            Layers[0].Properties[key] = "true";
        }

        public void InitializeSpecialLayer(Map.SpecialLayer value)
        {
            throw new NotImplementedException();
        }
    }
}
