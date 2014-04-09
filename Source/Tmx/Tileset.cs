using System;
using System.Collections.Generic;
using System.IO;
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

        public Tileset(XElement xTileset, string tmxFilePath = null)
        {
            // .tmx files have the concept of an external tileset which can be shared among various maps
            FirstGid = (int)xTileset.Attribute("firstgid");

            // If there is a attribute called source, the value indicates the file location of the external tileset
            if ((string)xTileset.Attribute("source") != null)
            {
                if (tmxFilePath == null)
                {
                    throw new ArgumentNullException("<Tileset::Tileset> : tmxFilePath must be set when an external tileset is referenced by the .tmx file.");
                }

                string tilesetFilePath = Path.Combine(Path.GetDirectoryName(tmxFilePath), (string)xTileset.Attribute("source"));
                XDocument xDocument = XDocument.Load(tilesetFilePath);
                xTileset = xDocument.Element("tileset");
            }

            Name = (string)xTileset.Attribute("name");
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

        public ReferenceTile FindReferenceTileByTile(Tile tile)
        {
            int referenceTileId = (int)tile.Gid - FirstGid;

            if (ReferenceTiles.ContainsKey(referenceTileId))
            {
                return ReferenceTiles[referenceTileId];
            }

            return null;
        }
    }
}
