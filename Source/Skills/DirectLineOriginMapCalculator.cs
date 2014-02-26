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
        public ISkill Skill { get; set; }

        public DirectLineOriginMapCalculator(ISkill skill)
        {
            Skill = skill;
        }

        public HashSet<Position> Calculate(IBoard board, Position skillUserPosition, Position targetPosition, bool allowDiagonalMovement = false)
        {
            HashSet<Position> returnValue = new HashSet<Position>();

            // Up
            AddPositionsInDirectLine(board, returnValue, skillUserPosition, targetPosition, 0, -1, Skill.Range);
            // Down
            AddPositionsInDirectLine(board, returnValue, skillUserPosition, targetPosition, 0, 1, Skill.Range);
            // Left
            AddPositionsInDirectLine(board, returnValue, skillUserPosition, targetPosition, -1, 0, Skill.Range);
            // Right
            AddPositionsInDirectLine(board, returnValue, skillUserPosition, targetPosition, 1, 0, Skill.Range);
            if (allowDiagonalMovement)
            {
                // UpLeft
                AddPositionsInDirectLine(board, returnValue, skillUserPosition, targetPosition, -1, -1, Skill.Range);
                // UpRight
                AddPositionsInDirectLine(board, returnValue, skillUserPosition, targetPosition, 1, -1, Skill.Range);
                // DownRight
                AddPositionsInDirectLine(board, returnValue, skillUserPosition, targetPosition, 1, 1, Skill.Range);
                // DownLeft
                AddPositionsInDirectLine(board, returnValue, skillUserPosition, targetPosition, -1, 1, Skill.Range);
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

        private void AddPositionsInDirectLine(IBoard board, HashSet<Position> positions, Position skillUserPosition, Position startingPosition, int deltaX, int deltaY, int range)
        {
            Position testingPosition;

            for (int i = 1; i <= range; i++)
            {
                testingPosition = new Position(startingPosition.X + i * deltaX, startingPosition.Y + i * deltaY);

                // If position is unwalkable, immediately return
                if (new Node(board, testingPosition.X, testingPosition.Y).IsWalkable() || testingPosition == skillUserPosition) 
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
