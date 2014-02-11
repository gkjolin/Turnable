using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using TurnItUp.Components;
using TurnItUp.Pathfinding;

namespace TurnItUp.Skills
{
    public class TargetMap : Dictionary<Tuple<int, int>, HashSet<Position>>
    {
    }
}
