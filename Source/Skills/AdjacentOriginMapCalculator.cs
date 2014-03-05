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
        public HashSet<Position> Calculate(IBoard board, Position skillUserPosition, Position targetPosition, int range, bool allowDiagonalMovement = false)
        {
            DirectLineOriginMapCalculator directLineOriginMapCalculator = new DirectLineOriginMapCalculator();

            return directLineOriginMapCalculator.Calculate(board, skillUserPosition, targetPosition, range, allowDiagonalMovement);
        }
    }
}
