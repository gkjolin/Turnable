using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Turnable.Tiled.Api;

namespace Turnable.Tiled.Internal
{
    internal class Data : IData
    {
        public Stream Contents { get; set; }

        public Data(XElement data)
        {
            // Decode the contents of data
            byte[] rawContents = Convert.FromBase64String((string)data.Value);
            Contents = new MemoryStream(rawContents, false);

            // Uncompress the decoded contents
            Contents = new Ionic.Zlib.ZlibStream(Contents, Ionic.Zlib.CompressionMode.Decompress, false);
        }
    }
}
