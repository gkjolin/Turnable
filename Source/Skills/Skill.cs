using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.Skills
{
    public class Skill
    {
        public RangeTypes RangeTypes { get; private set; }
        public TargetTypes TargetTypes { get; private set; }

        public Skill()
        {
            RangeTypes = Skills.RangeTypes.Adjacent;
            TargetTypes = Skills.TargetTypes.Enemies;
        }
    }
}
