using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TurnItUp.Tmx
{
    public class ReferenceTile
    {
        public int Id { get; private set; }
        public PropertyDictionary Properties { get; private set; }

        public ReferenceTile(int id)
        {
            Id = id;
        }

        public ReferenceTile(XElement xReferenceTile)
        {
            Id = (int)xReferenceTile.Attribute("id");

            // Load up the Properties for this reference tile, IF it exists
            if (xReferenceTile.Element("properties") != null)
            {
                IEnumerable<XElement> xProperties = xReferenceTile.Element("properties").Elements("property");
                Properties = new PropertyDictionary(xProperties);
            }
        }

    }
}
