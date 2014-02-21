using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TurnItUp.Tmx
{
    public class Data
    {
        public Stream Contents { get; private set; }

        public Data(XElement xData)
        {
            var encoding = (string)xData.Attribute("encoding");
            if (encoding != "base64")
            {
                throw new NotSupportedException("<Data::Data> : encodings other than Base64 are not supported for parsing .tmx files.");
            }

            // Decode the contents of xData
            byte[] rawContents = Convert.FromBase64String((string)xData.Value);
            Contents = new MemoryStream(rawContents, false);

            // Uncompress the decoded contents
            var compression = (string)xData.Attribute("compression");
            switch (compression)
            {
                case "gzip":
                    Contents = new GZipStream(Contents, CompressionMode.Decompress, false);
                    break;
                case "zlib":
                    Contents = new Ionic.Zlib.ZlibStream(Contents, Ionic.Zlib.CompressionMode.Decompress, false);
                    break;
            }
        }
    }
}
