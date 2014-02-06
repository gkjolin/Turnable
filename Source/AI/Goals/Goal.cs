using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.AI.Goals
{
    public abstract class Goal
    {
        public List<Goal> Subgoals { get; set; }
        public GoalStatus Status { get; set; }

        public virtual void Activate()
        {
            Status = GoalStatus.Active;
        }
        
        public virtual GoalStatus Process()
        {
            if (Status == GoalStatus.Inactive)
            {
                Activate();
            }

            return Status;
        }
        
        public abstract void Terminate();
        public abstract void AddSubgoal(Goal goal);
    }
}
