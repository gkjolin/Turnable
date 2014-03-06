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
    public class DirectLineOriginMapCalculator : ISkillOriginMapCalculator
    {
        public DirectLineOriginMapCalculator()
        {
        }

        public HashSet<Position> Calculate(ILevel level, Position skillUserPosition, Position targetPosition, int range, bool allowDiagonalMovement = false)
        {
            HashSet<Position> returnValue = new HashSet<Position>();

            // Up
            AddPositionsInDirectLine(level, returnValue, skillUserPosition, targetPosition, 0, -1, range);
            // Down
            AddPositionsInDirectLine(level, returnValue, skillUserPosition, targetPosition, 0, 1, range);
            // Left
            AddPositionsInDirectLine(level, returnValue, skillUserPosition, targetPosition, -1, 0, range);
            // Right
            AddPositionsInDirectLine(level, returnValue, skillUserPosition, targetPosition, 1, 0, range);
            if (allowDiagonalMovement)
            {
                // UpLeft
                AddPositionsInDirectLine(level, returnValue, skillUserPosition, targetPosition, -1, -1, range);
                // UpRight
                AddPositionsInDirectLine(level, returnValue, skillUserPosition, targetPosition, 1, -1, range);
                // DownRight
                AddPositionsInDirectLine(level, returnValue, skillUserPosition, targetPosition, 1, 1, range);
                // DownLeft
                AddPositionsInDirectLine(level, returnValue, skillUserPosition, targetPosition, -1, 1, range);
            }

            // Add back the skillUserPosition, since the skill user can always use the skill from its current position
            bool addSkillUserPosition = false;
            if (returnValue.Contains(skillUserPosition))
            {
                addSkillUserPosition = true;
            }

            if (addSkillUserPosition)
            {
                returnValue.Add(skillUserPosition);
            }

            return returnValue;
        }

        private void AddPositionsInDirectLine(ILevel level, HashSet<Position> positions, Position skillUserPosition, Position startingPosition, int deltaX, int deltaY, int range)
        {
            Position testingPosition;

            for (int i = 1; i <= range; i++)
            {
                testingPosition = new Position(startingPosition.X + i * deltaX, startingPosition.Y + i * deltaY);

                // If position is unwalkable, immediately return
                if (new Node(level, testingPosition.X, testingPosition.Y).IsWalkable() || testingPosition == skillUserPosition) 
                {
                    positions.Add(testingPosition);
                }
                else
                {
                    return;
                }
            }
        }
    }
}
