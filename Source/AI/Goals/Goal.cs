using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.AI.Goals
{
    public abstract class Goal : IComponent
    {
        public List<Goal> Subgoals { get; set; }
        public virtual GoalStatus Status { get; set; }
        public Entity Owner { get; set; }

        public virtual void Activate()
        {
            Status = GoalStatus.Active;
        }
        
        public virtual void Process()
        {
            if (Status == GoalStatus.Inactive)
            {
                Activate();
            }
        }
        
        public abstract void Terminate();
        public abstract void AddSubgoal(Goal goal);
    }
}
