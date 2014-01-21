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
        // TODO: Use a KeyedCollection here, it's more efficient
        public Dictionary<int, ReferenceTile> ReferenceTiles { get; private set; }

        public Tileset(XElement xTileset)
        {
            Name = (string)xTileset.Attribute("name");
            FirstGid = (int)xTileset.Attribute("firstgid");
            TileWidth = (int)xTileset.Attribute("tilewidth");
            TileHeight = (int)xTileset.Attribute("tileheight");
            Spacing = (int?)xTileset.Attribute("spacing") ?? 0;
            Margin = (int?)xTileset.Attribute("margin") ?? 0;
            ReferenceTiles = new Dictionary<int, ReferenceTile>();

            foreach (XElement xReferenceTile in xTileset.Elements("tile"))
            {
                ReferenceTile referenceTile = new ReferenceTile(xReferenceTile);

                ReferenceTiles[referenceTile.Id] = referenceTile;
            }
        }

        public ReferenceTile FindReferenceTileByProperty(string propertyName, string propertyValue)
        {
            foreach (ReferenceTile referenceTile in ReferenceTiles.Values)
            {
                if (referenceTile.Properties != null && referenceTile.Properties.ContainsKey(propertyName) && referenceTile.Properties[propertyName] == propertyValue)
                {
                    return referenceTile;
                }
            }

            return null;
        }
    }
}
