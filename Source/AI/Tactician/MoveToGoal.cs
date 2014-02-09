using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.AI.Goals;
using TurnItUp.Components;
using TurnItUp.Pathfinding;

namespace TurnItUp.AI.Tactician
{
    public class MoveToGoal : AtomicGoal
    {
        public Node Destination { get; private set; }

        public MoveToGoal(Entity character, Node destination)
        {
            Owner = character;
            Destination = destination;
        }

        public override void Process()
        {
            base.Process();

            Owner.GetComponent<OnBoard>().Board.MoveCharacterTo(Owner, Destination.Position);
        }
    }
}
