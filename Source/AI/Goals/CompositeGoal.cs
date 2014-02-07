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
            base.Activate();
        }

        public override void Process()
        {
            base.Process();

            // Terminate and remove all subgoals in the front of the subgoal list
            List<Goal> goalsToRemove = new List<Goal>();
            foreach (Goal goal in Subgoals)
            {
                if (goal.Status == GoalStatus.Completed || goal.Status == GoalStatus.Failed)
                {
                    goal.Terminate();
                    goalsToRemove.Add(goal);
                }
            }
            foreach (Goal goal in goalsToRemove)
            {
                Subgoals.Remove(goal);
            }

            if (Subgoals.Count > 0)
            {
                Subgoals[0].Process();
                Status = Subgoals[0].Status;

                // Keep this goal active even if the foremost subgoal completes, but if further subgoals exist
                if (Status == GoalStatus.Completed && Subgoals.Count >= 2)
                {
                    Status = GoalStatus.Active;
                }
            }
            else
            {
                
                Status = GoalStatus.Completed;
            }
        }

        public override void Terminate()
        {
            throw new NotImplementedException();
        }

        public override void AddSubgoal(Goal goal)
        {
            Subgoals.Insert(0, goal);
        }

        public virtual void RemoveAllSubgoals()
        {
            foreach (Goal goal in Subgoals)
            {
                goal.Terminate();
            }

            Subgoals.Clear();
        }
    }
}
