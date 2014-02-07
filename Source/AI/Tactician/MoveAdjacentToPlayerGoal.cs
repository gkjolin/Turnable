using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.AI.Goals;
using TurnItUp.Components;
using TurnItUp.Pathfinding;

namespace TurnItUp.AI.Tactician
{
    // TODO: Test this class!
    public class MoveAdjacentToPlayerGoal : CompositeGoal
    {
        public MoveAdjacentToPlayerGoal(Entity character)
        {
            Entity = character;
        }

        public override void Activate()
        {
            base.Activate();

            List<Node> bestPath = Entity.GetComponent<OnBoard>().Board.FindBestPathToMoveAdjacentToPlayer(Entity.GetComponent<Position>());
            Subgoals.Add(new FollowPathGoal(Entity, bestPath));
        }

        public override void Process()
        {
            base.Process();
        }
    }
}
