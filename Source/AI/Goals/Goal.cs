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

        public abstract void Activate();
        public abstract GoalStatus Process();
        public abstract void Terminate();
        public abstract void AddSubgoal(Goal goal);
    }
}
