using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.AI.Goals
{
    public class AtomicGoal : Goal
    {
        public AtomicGoal()
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
            throw new NotImplementedException();
        }
    }
}
