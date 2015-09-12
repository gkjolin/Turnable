using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Turnable.Tiled;

namespace Tests.Factories
{
    public static class TiledFactory
    {
        // Maps
        public static Map BuildMap()
        {
            return (new Map("c:/git/Turnable/Tests/Fixtures/FullExample.tmx"));
        }

        // Layers
        public static Layer BuildLayer()
        {
            return (new Layer(XDocument.Load("c:/git/Turnable/Tests/Fixtures/FullExample.tmx").Element("map").Elements("layer").First<XElement>()));
        }

        public static XElement BuildLayerXElement()
        {
            return (XDocument.Load("c:/git/Turnable/Tests/Fixtures/FullExample.tmx").Element("map").Elements("layer").First<XElement>());
        }

        public static XElement BuildLayerXElementWithNoProperties()
        {
            return (XDocument.Load("c:/git/Turnable/Tests/Fixtures/MinimalBase64ZlibCompressed.tmx").Element("map").Elements("layer").First<XElement>());
        }

        public static XElement BuildLayerXElementWithProperties()
        {
            return (XDocument.Load("c:/git/Turnable/Tests/Fixtures/FullExample.tmx").Element("map").Elements("layer").Last<XElement>());
        }

        // Tilesets
        public static Tileset BuildTileset()
        {
            return (new Tileset(XDocument.Load("c:/git/Turnable/Tests/Fixtures/FullExample.tmx").Element("map").Elements("tileset").Last<XElement>()));
        }

        public static XElement BuildTilesetXElement()
        {
            return (XDocument.Load("c:/git/Turnable/Tests/Fixtures/FullExample.tmx").Element("map").Elements("tileset").First<XElement>());
        }

        //public static XElement BuildExternalTilesetXElement()
        //{
        //    return (XDocument.Load("../../Fixtures/FullExampleWithExternalTilesetReference.tmx").Element("map").Elements("tileset").Last<XElement>());
        //}

        public static XElement BuildTilesetXElementWithSpecialTiles()
        {
            return (XDocument.Load("c:/git/Turnable/Tests/Fixtures/FullExample.tmx").Element("map").Elements("tileset").Last<XElement>());
        }

        // Data
        public static Data BuildDataWithNoTiles()
        {
            return (new Data(XDocument.Load("c:/git/Turnable/Tests/Fixtures/MinimalBase64ZlibCompressed.tmx").Element("map").Elements("layer").First<XElement>().Element("data")));
        }

        public static Data BuildDataWithTiles()
        {
            return (new Data(XDocument.Load("c:/git/Turnable/Tests/Fixtures/FullExample.tmx").Element("map").Elements("layer").Last<XElement>().Element("data")));
        }

        // Properties
        public static IEnumerable<XElement> BuildPropertiesXElements()
        {
            return (XDocument.Load("c:/git/Turnable/Tests/Fixtures/FullExample.tmx").Element("map").Elements("layer").First<XElement>().Element("properties").Elements("property"));
        }

        // Special Tiles
        public static XElement BuildSpecialTileXElementWithProperties()
        {
            return (XDocument.Load("c:/git/Turnable/Tests/Fixtures/FullExample.tmx").Element("map").Elements("tileset").First<XElement>().Elements("tile").First<XElement>());
        }
    }
}
