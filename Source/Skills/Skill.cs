using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using TurnItUp.Locations;

namespace TurnItUp.Skills
{
    public class Skill : ISkill
    {
        public string Name { get; set; }
        public RangeType RangeType { get; set; }
        public TargetType TargetType { get; set; }
        public int Range { get; set; }
        public List<IEffect> Effects { get; private set; }
        public ISkillOriginMapCalculator OriginMapCalculator { get; set; }

        public Skill(string name) : this(name, RangeType.Adjacent, TargetType.InAnotherTeam, 1)
        {
        }

        public Skill(string name, RangeType rangeType, TargetType targetType, int range)
        {
            Name = name;
            RangeType = rangeType;
            TargetType = targetType;
            Range = range;
            Effects = new List<IEffect>();

            switch (rangeType)
            {
                case RangeType.Adjacent:
                    OriginMapCalculator = new AdjacentOriginMapCalculator(this);
                    break;
                case RangeType.DirectLine:
                    OriginMapCalculator = new DirectLineOriginMapCalculator(this);
                    break;
                default:
                    break;
            }
        }

        public TargetMap CalculateTargetMap(IBoard board, Position skillUserPosition)
        {
            TargetMap returnValue = new TargetMap();

            Position playerPosition = board.CharacterManager.Player.GetComponent<Position>();

            HashSet<Position> originMap = OriginMapCalculator.Calculate(board, skillUserPosition, playerPosition, board.PathFinder.AllowDiagonalMovement);
            returnValue.Add(new System.Tuples.Tuple<int, int>(playerPosition.X, playerPosition.Y), originMap);
            return returnValue;
        }

        public void Apply(Entity user, Entity target)
        {
            foreach (IEffect effect in Effects)
            {
                effect.Apply(user, target);
            }
        }
    }
}
