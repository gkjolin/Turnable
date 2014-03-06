using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using TurnItUp.Locations;
using TurnItUp.Pathfinding;

namespace TurnItUp.Skills
{
    public class AdjacentOriginMapCalculator : ISkillOriginMapCalculator
    {
        public HashSet<Position> Calculate(ILevel level, Position skillUserPosition, Position targetPosition, int range, bool allowDiagonalMovement = false)
        {
            DirectLineOriginMapCalculator directLineOriginMapCalculator = new DirectLineOriginMapCalculator();

            return directLineOriginMapCalculator.Calculate(level, skillUserPosition, targetPosition, range, allowDiagonalMovement);
        }
    }
}
