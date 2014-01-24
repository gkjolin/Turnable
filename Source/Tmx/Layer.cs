using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using System.Xml.Linq;

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

        public Layer(XElement xLayer, int width, int height)
        {
            Name = (string)xLayer.Attribute("name");
            Opacity = (double?)xLayer.Attribute("opacity") ?? 1.0;
            IsVisible = (bool?)xLayer.Attribute("visible") ?? true;
            Height = height;
            Width = width;

            // Load up the Tiles in this layer
            Data data = new Data(xLayer.Element("data"));
            Tiles = new TileList(data, height, width);

            // Load up the Properties for this layer, IF it exists
            if (xLayer.Element("properties") != null)
            {
                IEnumerable<XElement> xProperties = xLayer.Element("properties").Elements("property");
                Properties = new PropertyDictionary(xProperties);
            }
        }

        //public void MoveTile(int oldX, int oldY, int newX, int newY)
        //{
        //    if (!Tiles.ContainsKey(new System.Tuples.Tuple<int, int>(oldX, oldY)))
        //    {
        //        throw new InvalidOperationException(String.Format("A tile that does not exist at: {0}, {1}", oldX.ToString(), oldY.ToString()));
        //    }

        //    if (Tiles.ContainsKey(new System.Tuples.Tuple<int, int>(newX, newY)))
        //    {
        //        throw new InvalidOperationException(String.Format("A tile already exists at: {0}, {1}", newX.ToString(), newY.ToString()));
        //    }

        //    Tiles[new Tuple<int, int>(newX, newY)] = new Tile(Tiles[new Tuple<int, int>(oldX, oldY)].Gid, newX, newY);
        //    UnsetTile(oldX, oldY);
        //}

        //public void UnsetTile(int x, int y)
        //{
        //    if (Tiles.ContainsKey(new System.Tuples.Tuple<int,int>(x, y)))
        //    {
        //        Tiles.Remove(new System.Tuples.Tuple<int,int>(x, y));
        //    }
        //}
    }
}
