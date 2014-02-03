using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.AI.Goals
{
    public class CompositeGoal : Goal
    {
        public CompositeGoal()
        {
            Subgoals = new List<Goal>();
        }

        public override void Activate()
        {
            throw new NotImplementedException();
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }

        public override void Terminate()
        {
            throw new NotImplementedException();
        }

        public override void AddSubgoal(Goal goal)
        {
            Subgoals.Insert(0, goal);
        }
    }
}
