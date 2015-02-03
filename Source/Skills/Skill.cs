using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;

namespace Turnable.Skills
{
    public class Skill
    {
        public string Name { get; set; }
        public TargetType TargetType { get; set; }
        public RangeType RangeType { get; set; }
        public int Cost { get; set; }
        public int Range { get; set; }
        public IEnumerable<IEffect> Effects { get; set; }

        public Skill(string name) : this(name, RangeType.Adjacent, TargetType.InAnotherTeam, 0, 0)
        {
        }

        public Skill(string name, RangeType rangeType, TargetType targetType, int range, int cost)
        {
            Name = name;
            TargetType = targetType;
            RangeType = rangeType;
            Effects = new List<IEffect>();
            Range = range;
            Cost = cost;
        }
    }
}
