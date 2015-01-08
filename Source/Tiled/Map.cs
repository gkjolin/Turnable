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
        public SpecialLayersCollection SpecialLayers { get; set; }

        public Map()
        {
            SpecialLayers = new SpecialLayersCollection();
        }

        public Map(string tmxFullFilePath) : this()
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

        public class SpecialLayersCollection : Dictionary<Map.SpecialLayer, Layer>
        {
            public new Layer this[Map.SpecialLayer index]
            {
                get
                {
                    Layer returnValue;

                    base.TryGetValue(index, out returnValue);

                    return returnValue;
                }

                set
                {
                    string key = SpecialLayerPropertyKey(index);

                    if (ContainsKey(index))
                    {
                        throw new ArgumentException();
                    }

                    value.Properties[key] = "true";
                    base[index] = value;
                }
            }

            private string SpecialLayerPropertyKey(SpecialLayer specialLayer)
            {
                return "Is" + specialLayer.ToString() + "Layer";
            }
        }
    }
}
