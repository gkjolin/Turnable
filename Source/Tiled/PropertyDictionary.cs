using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Turnable.Tiled
{
    public class PropertyDictionary : Dictionary<string, string>
    {
        public PropertyDictionary()
        {
        }

        public PropertyDictionary(IEnumerable<XElement> xProperties) : base(StringComparer.OrdinalIgnoreCase)
        {
            foreach (XElement xProperty in xProperties)
            {
                Add((string)xProperty.Attribute("name"), (string)xProperty.Attribute("value"));
            }
        }

        public new string this[string index]
        {
            get 
            {
                string returnValue;

                TryGetValue(index, out returnValue);

                return returnValue;
            }

            set
            {
                base[index] = value;
            }
        }
    }
}
