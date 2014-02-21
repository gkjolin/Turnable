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
        public HashSet<Position> Calculate(IBoard board, Position skillUserPosition, Position targetPosition, int range, bool allowDiagonalMovement = false, bool lineOfSightRequired = true)
        {
            HashSet<Position> returnValue = new HashSet<Position>();

            for (int x = targetPosition.X - 1; x <= targetPosition.X + 1; x++)
            {
                for (int y = targetPosition.Y - 1; y <= targetPosition.Y + 1; y++)
                {
                    if (x == targetPosition.X && y == targetPosition.Y) continue;

                    returnValue.Add(new Position(x, y));
                }
            }

            // Add back the skillUserPosition, since the skill user can always use the skill from its current position
            bool addSkillUserPosition = false;
            if (returnValue.Contains(skillUserPosition))
            {
                addSkillUserPosition = true;
            }

            // Remove unwalkable positions
            returnValue.RemoveWhere(p => !(new Node(board, p.X, p.Y)).IsWalkable());

            // Remove non-orthogonal nodes if needed
            if (!allowDiagonalMovement)
            {
                returnValue.RemoveWhere(p => !(p.X == targetPosition.X || p.Y == targetPosition.Y));
            }

            if (addSkillUserPosition)
            {
                returnValue.Add(skillUserPosition);
            }

            return returnValue;
        }
    }
}
