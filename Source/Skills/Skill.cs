using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turnable.Skills
{
    public class Skill
    {
        public string Name { get; set; }
        public TargetType TargetType { get; set; }
        public RangeType RangeType { get; set; }
        public int Cost { get; set; }
        public int Range { get; set; }

        public Skill(string name) 
        {
            Name = name;
            TargetType = Skills.TargetType.InOtherTeam;
            RangeType = Skills.RangeType.Adjacent;
        }
    }
}
