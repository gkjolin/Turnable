using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Turnable.Tiled.Api;

namespace Turnable.Tiled
{
    public class Layer : ILayer
    {
        public bool? IsVisible { get; set; }
        public string Name { get; set; }
        public double? Opacity { get; set; }

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

            /*

                        // Load up the Tiles in this layer
                        Data data = new Data(xLayer.Element("data"));
                        Tiles = new TileList(Width, Height, data);

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
