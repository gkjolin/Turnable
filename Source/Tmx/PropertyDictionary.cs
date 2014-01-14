using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TurnItUp.Tmx
{
    public class PropertyDictionary : Dictionary<string, string>
    {
        public PropertyDictionary(IEnumerable<XElement> xProperties)
        {
            foreach (XElement xProperty in xProperties)
            {
                Add((string)xProperty.Attribute("name"), (string)xProperty.Attribute("value"));
            }
        }
    }
}
