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
    public interface IStat
    {
        // ----------------
        // Public interface
        // ----------------
        void Reset();

        // ----------
        // Properties
        // ----------
        int MinimumValue { get; set; }
        int MaximumValue { get; set; }
        string Name { get; set; }
        int Value { get; set; }

        // -----------------
        // Private interface
        // -----------------
    }
}