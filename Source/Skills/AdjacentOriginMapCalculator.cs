using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;
using TurnItUp.Locations;
using TurnItUp.Pathfinding;

namespace TurnItUp.Skills
{
    public static class AdjacentOriginMapCalculator
    {
        public static HashSet<Position> CalculateOriginMap(Board board, Position targetPosition, bool allowDiagonalMovement = false)
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

            // Remove unwalkable positions
            returnValue.RemoveWhere(p => !(new Node(board, p.X, p.Y)).IsWalkable());

            return returnValue;
        }
    }
}
