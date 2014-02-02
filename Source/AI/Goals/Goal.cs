using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.AI.Goals
{
    public class Goal
    {
        public List<Goal> Subgoals { get; private set; }

        public Goal()
        {
            Subgoals = new List<Goal>();
        }
    }
}
