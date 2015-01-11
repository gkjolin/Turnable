using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Turnable.Tiled
{
    public class SpecialTile
    {
        public Tileset Tileset { get; set; }
        public uint Id { get; set; }
        public PropertyDictionary Properties { get; set; }

        public SpecialTile(Tileset tileset, uint id)
        {
            Tileset = tileset;
            Id = id;
        }

        public SpecialTile(Tileset tileset, XElement xSpecialTile)
            : this(tileset, (uint)xSpecialTile.Attribute("id"))
        {
            Properties = new PropertyDictionary();

            // Load up the Properties for this reference tile
            if (xSpecialTile.Element("properties") != null)
            {
                IEnumerable<XElement> xProperties = xSpecialTile.Element("properties").Elements("property");
                Properties = new PropertyDictionary(xProperties);
            }
        }

        public uint GlobalId 
        {
            get 
            {
                return Id + Tileset.FirstGlobalId;
            }
        }
    }
}
