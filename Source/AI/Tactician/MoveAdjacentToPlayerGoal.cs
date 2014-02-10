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
            Owner = character;
        }

        public override void Activate()
        {
            base.Activate();

            List<Node> bestPath = Owner.GetComponent<OnBoard>().Board.FindBestPathToMoveAdjacentToPlayer(Owner.GetComponent<Position>());

            if (bestPath == null)
            {
                Status = GoalStatus.Failed;
            }
            else
            {
                Subgoals.Add(new FollowPathGoal(Owner, bestPath));
            }
        }

        public override void Process()
        {
            base.Process();
        }

        public override void Terminate()
        {
            base.Terminate();

            // TODO: Test this
            Owner.GetComponent<OnBoard>().Board.CharacterManager.EndTurn();
        }
    }
}
