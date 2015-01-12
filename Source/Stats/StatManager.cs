using Entropy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;

namespace Turnable.Stats
{
    public class StatManager : IStatManager, IComponent
    {
        public Dictionary<string, Stat> Stats { get; set; }

        public StatManager()
        {
            Stats = new Dictionary<string, Stat>(StringComparer.OrdinalIgnoreCase);
        }

        public Stat BuildStat(string name, int initialValue, int minimumValue = 0, int maximumValue = 100)
        {
            Stat newStat = new Stat(name, initialValue, maximumValue, minimumValue);
            Stats.Add(name, newStat);

            return newStat;
        }

        public Stat GetStat(string name)
        {
            Stat stat;

            Stats.TryGetValue(name, out stat);

            return stat;
        }
    }
}
