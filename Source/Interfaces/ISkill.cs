using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Locations;
using TurnItUp.Skills;

namespace TurnItUp.Interfaces
{
    public interface ISkill
    {
        string Name { get; set; }
        RangeType RangeType { get; set; }
        TargetType TargetType { get; set; }
        int Range { get; set; }

        TargetMap CalculateTargetMap(Board board);
    }
}
