using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.AI.Goals;

namespace TurnItUp.AI.Tactician
{
    public class MoveToClosestNodeInListOfNodesGoal : CompositeGoal
    {
        public MoveToClosestNodeInListOfNodesGoal()
        {
            Subgoals.Add(new FindClosestNodeInListOfNodesGoal());
            Subgoals.Add(new FindPathBetweenNodesGoal());
            Subgoals.Add(new MoveAlongPathGoal());
        }
    }
}
