using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Turnable.Tiled.Api
{
    public interface IData
    {
        Stream Contents { get; set; }
    }
}
