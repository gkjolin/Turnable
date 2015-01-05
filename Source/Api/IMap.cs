using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Tiled;

namespace Turnable.Api
{
    public interface IMap
    {
        int TileWidth { get; set; }
        int TileHeight { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        RenderOrder RenderOrder { get; set; }
        Orientation Orientation { get; set; }
        string Version { get; set; }
        ElementList<Layer> Layers { get; set; }

        public void SetSpecialLayer(Layer layer, Map.SpecialLayer value);
        public void InitializeSpecialLayer(Map.SpecialLayer value);
    }
}

//namespace Turnable.Tiled
//{
//    public class Map : IMap
//    {
//        public Map()
//        {
//        }

//        public Map(string tmxFullFilePath)
//        {
//            XDocument xDocument = XDocument.Load(tmxFullFilePath);
//            XElement xMap = xDocument.Element("map");

//            TileWidth = (int)xMap.Attribute("tilewidth");
//            TileHeight = (int)xMap.Attribute("tileheight");
//            Width = (int)xMap.Attribute("width");
//            Height = (int)xMap.Attribute("height");
//            // TODO: Load up RenderOrder correctly
//            Orientation = (Orientation)Enum.Parse(typeof(Orientation), xMap.Attribute("orientation").Value, true);
//            Version = (string)xMap.Attribute("version");

//            // Load up all the Layers in this Map.
//            Layers = new ElementList<Layer>();
//            foreach (XElement xLayer in xMap.Elements("layer"))
//            {
//                Layers.Add(new Layer(xLayer));
//            }
//        }

//        public enum SpecialLayer
//        {
//            Background,
//            Collision,
//            Object,
//            Character
//        }

//        public void SetSpecialLayer(Layer layer, SpecialLayer value)
//        {
//            string key = "Is" + value.ToString() + "Layer";

//            if (Layers[0].Properties.ContainsKey(key))
//            {
//                throw new ArgumentException();
//            }
            
//            Layers[0].Properties[key] = "true";
//        }
//    }
//}
