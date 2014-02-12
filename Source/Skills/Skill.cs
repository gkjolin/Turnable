using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;
using TurnItUp.Locations;

namespace TurnItUp.Skills
{
    public class Skill
    {
        public RangeTypes RangeTypes { get; set; }
        public TargetTypes TargetTypes { get; set; }
        public int Range { get; set; }

        public Skill()
        {
            RangeTypes = Skills.RangeTypes.Adjacent;
            TargetTypes = Skills.TargetTypes.InAnotherTeam;
            Range = 1;
        }

        public TargetMap CalculateTargetMap(Board board)
        {
            return null;
        }
    }
}
