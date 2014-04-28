using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entropy;
using TurnItUp.Stats;
using TurnItUp.Interfaces;

namespace TurnItUp.Skills
{
    public class StatChangeEffect : IEffect
    {
        public Dictionary<string, string> StatChanges { get; private set; }

        public StatChangeEffect()
        {
            StatChanges = new Dictionary<string, string>();
        }

        public void AddStatChange(string name, string change)
        {
            if (StatChanges.ContainsKey(name)) throw new InvalidOperationException(String.Format("<StatChangeEffect::AddStatChange> : you added a change for {0} when it already exists.", name));
            StatChanges[name] = change;
        }

        public void Apply(Entity executor, Entity target)
        {
            foreach (string name in StatChanges.Keys)
            {
                target.GetComponent<StatManager>().GetStat(name).Value += Convert.ToInt32(StatChanges[name]);
            }
        }
    }
}
