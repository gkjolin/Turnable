using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.AI.Goals;
using TurnItUp.AI.Tactician;

namespace TurnItUp.AI.Brains
{
    public class Brain : CompositeGoal
    {
        public Goal CurrentGoal { get; private set; }

        public Brain() : this(null)
        {
        }

        public Brain(Entity character)
        {
            Owner = character;
        }

        public void Think()
        {
            CurrentGoal = new MoveAdjacentToPlayerGoal(Owner);
        }
    }
}
