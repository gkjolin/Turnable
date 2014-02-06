using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.AI.Goals;
using TurnItUp.Pathfinding;

namespace TurnItUp.AI.Tactician
{
    public class FollowPathGoal : CompositeGoal
    {
        public List<Node> Path { get; private set; }

        public FollowPathGoal(List<Node> path)
        {
            Path = path;
        }
    }
}
