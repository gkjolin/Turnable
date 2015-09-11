using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;
using Turnable.Locations;
using Turnable.Stats;

namespace Turnable.Api
{
    public interface IStat
    {
        // ----------------
        // Public Interface
        // ----------------

        // Methods
        void Reset();

        // Properties
        int MinimumValue { get; set; }
        int MaximumValue { get; set; }
        string Name { get; set; }
        int Value { get; set; }

        // Events
        event EventHandler<StatChangedEventArgs> Changed;

        // -----------------
        // Private interface
        // -----------------
    }
}