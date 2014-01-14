using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TurnItUp.Tmx
{
    public class Tileset : IElement
    {
        public string Name { get; private set; }
        public int FirstGid { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public int Spacing { get; private set; }
        public int Margin { get; private set; }

        public Tileset(XElement xTileset)
        {
            Name = (string)xTileset.Attribute("name");
            FirstGid = (int)xTileset.Attribute("firstgid");
            TileWidth = (int)xTileset.Attribute("tilewidth");
            TileHeight = (int)xTileset.Attribute("tileheight");
            Spacing = (int?)xTileset.Attribute("spacing") ?? 0;
            Margin = (int?)xTileset.Attribute("margin") ?? 0;
        }
    }
}
