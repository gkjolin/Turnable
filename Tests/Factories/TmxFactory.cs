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
        public static Layer BuildLayer()
        {
            return (new Layer(XDocument.Load("../../Fixtures/FullExample.tmx").Element("map").Elements("layer").First<XElement>(), 15, 15));
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

    }
}
