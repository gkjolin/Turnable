using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.AI.Goals;
using TurnItUp.Pathfinding;

namespace TurnItUp.AI.Tactician
{
    public class MoveToGoal : AtomicGoal
    {
        public Node Destination { get; private set; }

        public MoveToGoal(Entity character, Node destination)
        {
            Entity = character;
            Destination = destination;
        }
    }
}
