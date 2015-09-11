using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;
using Turnable.Locations;
using Turnable.Utilities;

namespace Turnable.Api
{
    public interface IBounded
    {
        // ----------------
        // Public interface
        // ----------------

        // Methods

        // Properties
        Rectangle Bounds { get; set; }

        // Events

        // -----------------
        // Private interface
        // -----------------
    }
}
