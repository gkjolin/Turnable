using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.AI.Goals;
using TurnItUp.AI.Tactician;

namespace TurnItUp.AI.Brains
{
    public class Brain : IComponent
    {
        public Entity Entity { get; set; }
        public Goal CurrentGoal { get; set; }

        public Brain()
        {
        }

        public void WakeUp()
        {
            CurrentGoal = new MoveNextToCharacterGoal();
        }
    }
}
