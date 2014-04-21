using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Tmx;
using System.Tuples;
using TurnItUp.Characters;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using Entropy;
using TurnItUp.Pathfinding;
using TurnItUp.Randomization;

namespace TurnItUp.Locations
{
    public class Connection
    {
        public Node StartNode { get; set; }
        public Node EndNode { get; set; }

        public Connection(Node startNode, Node endNode)
        {
            StartNode = startNode;
            EndNode = endNode;
        }
    }
}
