using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Turnable.Tiled.Api;

namespace Turnable.Tiled
{
    public class Map : IMap
    {
        public int? Height { get; set; }
        public Orientation Orientation { get; set; }
        public RenderOrder RenderOrder { get; set; }
        public int? TileHeight { get; set; }
        public int? TileWidth { get; set; }
        public string Version { get; set; }
        public int? Width { get; set; }

        public Map()
        {
        }

        public Map(string tmxFullFilePath) : this()
        {
            if (!File.Exists(tmxFullFilePath))
            {
                throw new ArgumentException();
            }

            var xDocument = XDocument.Load(tmxFullFilePath);
            var xMap = xDocument.Element("map");

            TileWidth = (int?)xMap.Attribute("tilewidth");
            TileHeight = (int?)xMap.Attribute("tileheight");
            Width = (int?)xMap.Attribute("width");
            Height = (int?)xMap.Attribute("height");
            // TODO: Load up RenderOrder correctly
            Orientation = (Orientation)Enum.Parse(typeof(Orientation), xMap.Attribute("orientation").Value, true);
            Version = (string)xMap.Attribute("version");
        }
    }
}
