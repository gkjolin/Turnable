using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TurnItUp.Tmx;

namespace Tests.Factories
{
    public static class TmxFactory
    {
        public static Map BuildMap()
        {
            return (new Map("../../Fixtures/FullExample.tmx"));
        }

        public static Layer BuildLayer()
        {
            return (new Layer(XDocument.Load("../../Fixtures/FullExample.tmx").Element("map").Elements("layer").First<XElement>(), 16, 16));
        }

        public static Tileset BuildTileset()
        {
            return (new Tileset(XDocument.Load("../../Fixtures/FullExample.tmx").Element("map").Elements("tileset").Last<XElement>()));
        }

        public static XElement BuildTilesetXElement()
        {
            return (XDocument.Load("../../Fixtures/FullExample.tmx").Element("map").Elements("tileset").First<XElement>());
        }

        public static XElement BuildExternalTilesetXElement()
        {
            return (XDocument.Load("../../Fixtures/FullExampleWithExternalTilesetReference.tmx").Element("map").Elements("tileset").Last<XElement>());
        }

        public static XElement BuildTilesetXElementWithReferenceTiles()
        {
            return (XDocument.Load("../../Fixtures/FullExample.tmx").Element("map").Elements("tileset").Last<XElement>());
        }

        public static XElement BuildLayerXElement()
        {
            return (XDocument.Load("../../Fixtures/FullExample.tmx").Element("map").Elements("layer").First<XElement>());
        }

        public static XElement BuildLayerXElementWithProperties()
        {
            return (XDocument.Load("../../Fixtures/FullExample.tmx").Element("map").Elements("layer").Last<XElement>());
        }

        public static IEnumerable<XElement> BuildPropertiesXElements()
        {
            return (XDocument.Load("../../Fixtures/FullExample.tmx").Element("map").Elements("layer").Last<XElement>().Element("properties").Elements("property"));
        }

        public static Data BuildDataWithNoTiles()
        {
            return (new Data(XDocument.Load("../../Fixtures/MinimalBase64Zlib.tmx").Element("map").Elements("layer").First<XElement>().Element("data")));
        }

        public static Data BuildDataWithTiles()
        {
            return (new Data(XDocument.Load("../../Fixtures/FullExample.tmx").Element("map").Elements("layer").First<XElement>().Element("data")));
        }

        public static XElement BuildReferenceTileXElementWithProperties()
        {
            return (XDocument.Load("../../Fixtures/FullExample.tmx").Element("map").Elements("tileset").Last<XElement>().Elements("tile").First<XElement>());
        }
    }
}
