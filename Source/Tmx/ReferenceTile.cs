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
        public uint Id { get; private set; }
        public Tileset Tileset { get; private set; }
        public PropertyDictionary Properties { get; private set; }

        public ReferenceTile(Tileset tileset, uint id)
        {
            Id = id;
            Tileset = tileset;
        }

        public ReferenceTile(Tileset tileset, XElement xReferenceTile) : this(tileset, (uint)xReferenceTile.Attribute("id"))
        {
            // Load up the Properties for this reference tile, IF it exists
            if (xReferenceTile.Element("properties") != null)
            {
                IEnumerable<XElement> xProperties = xReferenceTile.Element("properties").Elements("property");
                Properties = new PropertyDictionary(xProperties);
            }
        }

        public uint Gid
        {
            get
            {
                return Id + Tileset.FirstGid;
            }
        }
    }
}
