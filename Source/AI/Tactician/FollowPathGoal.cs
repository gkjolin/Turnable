using Entropy;
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

        public FollowPathGoal(Entity character, List<Node> path)
        {
            Entity = character;
            Path = path;
        }

        public override void Activate()
        {
            base.Activate();

            AddSubgoal(new MoveToGoal(Entity, Path[1]));
        }
    }
}
