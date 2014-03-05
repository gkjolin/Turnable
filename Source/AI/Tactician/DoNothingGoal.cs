using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.AI.Goals;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using TurnItUp.Pathfinding;
using TurnItUp.Skills;

namespace TurnItUp.AI.Tactician
{
    // TODO: Unit test this!
    public class DoNothingGoal : AtomicGoal
    {
        public DoNothingGoal(Entity character)
        {
            Owner = character;
        }

        public override void Activate()
        {
            base.Activate();
        }

        public override void Process()
        {
            base.Process();
        }
    }
}
