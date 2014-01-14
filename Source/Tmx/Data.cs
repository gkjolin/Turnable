using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TurnItUp.Tmx
{
    public class Data
    {
        public byte[] Contents { get; private set; }

        public Data(XElement xData)
        {
            var encoding = (string)xData.Attribute("encoding");
            if (encoding != "base64")
            {
                throw new NotSupportedException("Encodings other than Base64 are not supported for parsing .tmx files.");
            }

            var compression = (string)xData.Attribute("compression");
            if (compression != "gzip")
            {
                throw new NotSupportedException("Compression methods other than gzip are not supported for parsing .tmx files.");
            }
        }
    }
}
