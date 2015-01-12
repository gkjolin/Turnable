using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using Turnable.Components;
using Turnable.Locations;

namespace Turnable.Api
{
    public interface ICharacterManager
    {
        // ----------------
        // Public interface
        // ----------------

        // ----------
        // Properties
        // ----------
        ILevel Level { get; set; }

        // -----------------
        // Private interface
        // -----------------
    }
}
