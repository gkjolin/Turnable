using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using Turnable.Components;
using Turnable.Locations;
using Turnable.Stats;

namespace Turnable.Api
{
    public interface IStatManager
    {
        // ----------------
        // Public interface
        // ----------------

        // Methods
        Stat BuildStat(string name, int initialValue, int minimumValue = 0, int maximumValue = 100);
        Stat GetStat(string name);

        // Properties
        Dictionary<string, Stat> Stats { get; set; }

        // -----------------
        // Private interface
        // -----------------
    }
}
