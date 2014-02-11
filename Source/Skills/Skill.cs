using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;

namespace TurnItUp.Skills
{
    public class Skill
    {
        public RangeTypes RangeTypes { get; set; }
        public TargetTypes TargetTypes { get; set; }

        public Skill()
        {
            RangeTypes = Skills.RangeTypes.Adjacent;
            TargetTypes = Skills.TargetTypes.Enemies;
        }

        public TargetMap CalculateTargetMap(Position skillUserPosition)
        {
            TargetMap returnValue = new TargetMap();

            if ((TargetTypes & TargetTypes.Self) == TargetTypes.Self) 
            {
                HashSet<Position> possibleTargetPositions = new HashSet<Position>();

                possibleTargetPositions.Add(skillUserPosition.DeepClone());
                returnValue.Add(new System.Tuples.Tuple<int, int>(skillUserPosition.X, skillUserPosition.Y), possibleTargetPositions);
            }

            return returnValue;
        }
    }
}
