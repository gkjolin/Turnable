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
        public string Name { get; set; }
        public RangeType RangeType { get; set; }
        public TargetType TargetType { get; set; }
        public int Range { get; set; }

        public Skill(string name) : this(name, RangeType.Adjacent, TargetType.InAnotherTeam, 1)
        {
        }

        public Skill(string name, RangeType rangeType, TargetType targetType, int range)
        {
            Name = name;
            RangeType = rangeType;
            TargetType = targetType;
            Range = range;
        }

        public TargetMap CalculateTargetMap(Board board)
        {
            TargetMap returnValue = new TargetMap();

            Position playerPosition = board.CharacterManager.Player.GetComponent<Position>();

            HashSet<Position> originMap = AdjacentOriginMapCalculator.CalculateOriginMap(board, playerPosition, board.PathFinder.AllowDiagonalMovement);
            returnValue.Add(new System.Tuples.Tuple<int, int>(playerPosition.X, playerPosition.Y), originMap);
            return returnValue;
        }
    }
}
