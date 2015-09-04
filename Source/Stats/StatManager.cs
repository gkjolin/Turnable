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
        public Entity Owner { get; set; }

        public StatManager()
        {
            Stats = new Dictionary<string, Stat>(StringComparer.OrdinalIgnoreCase);
        }

        public Stat BuildStat(string name, int initialValue, int minimumValue = 0, int maximumValue = 100)
        {
            Stat newStat = new Stat(name, initialValue, maximumValue, minimumValue);
            Stats.Add(name, newStat);
            newStat.Changed += OnStatChanged;

            return newStat;
        }

        protected virtual void OnStatChanged(object sender, StatChangedEventArgs e)
        {
            if (StatChanged != null)
            {
                StatChanged(this, e);
            }
        }

        public Stat GetStat(string name)
        {
            Stat stat;

            Stats.TryGetValue(name, out stat);

            return stat;
        }

        public event EventHandler<StatChangedEventArgs> StatChanged;
    }
}
