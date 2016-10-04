using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turnable.Tiled.Api
{
    public interface IMap
    {
        // Methods

        // Properties
        string Version { get; set; }
        Orientation Orientation { get; set; }
        RenderOrder RenderOrder { get; set; }

        // Events
    }
}
