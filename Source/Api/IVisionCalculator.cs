using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using Turnable.Components;
using Turnable.Locations;
using Turnable.Tiled;

namespace Turnable.Api
{
    public interface IVisionCalculator
    {
        // ----------------
        // Public interface
        // ----------------

        // Properties
        ILevel Level { get; set; }

        // -----------------
        // Private interface
        // -----------------
    }
}
