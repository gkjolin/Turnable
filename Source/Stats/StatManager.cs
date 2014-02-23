using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entropy;
using System.Text.RegularExpressions;

namespace TurnItUp.Stats
{
    public class StatManager : IComponent
    {
        public Entity Owner { get; set; }
        public List<Stat> Stats { get; private set; }

        public StatManager()
        {
            Stats = new List<Stat>();
        }

        public Stat CreateStat(string name, int initialValue, int minimumValue = 0, int maximumValue = 100)
        {
            if (Stats.FindAll(a => a.Name.ToLower() == name.ToLower()).Count != 0) throw new ArgumentException(string.Format("<StatManager::CreateStat> : {0} stat already exists.", name));
            Stat stat = new Stat(name, initialValue, minimumValue, maximumValue);
            Stats.Add(stat);
            stat.Changed += OnStatChanged;
            return stat;
        }

        public Stat GetStat(string name)
        {
            List<Stat> results = Stats.FindAll(a => a.Name == name);
            if (results.Count == 0) return null;
            return results[0];
        }

        public virtual event EventHandler<StatChangedEventArgs> StatChanged;

        protected virtual void OnStatChanged(object sender, StatChangedEventArgs e)
        {
            if (StatChanged != null)
            {
                StatChanged(this, new StatChangedEventArgs(Owner, e.Stat));
            }
        }

        //private string ToCanonicalName(string name)
        //{
        //    // Make the name canonical, i.e all lowercase and replacing the spaces with underscores
        //    string returnValue = name.ToLower().Replace(" ", "_");
        //    return returnValue;
        //}
    }
}
