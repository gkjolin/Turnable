using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Locations;
using TurnItUp.Pathfinding;

namespace TurnItUp.Fov
{
    public class FovCalculator
    {
        public Level Level { get; set; }

        public FovCalculator(Level level)
        {
            Level = level;
        }

        public double CalculateSlope(Node node1, Node node2, bool inverse = false)
        {
            if (!inverse)
            {
                return (((double)node1.Position.X - (double)node2.Position.X) / ((double)node1.Position.Y - (double)node2.Position.Y));
            }
            else
            {
                return 1.0 / CalculateSlope(node1, node2);
            }
        }

        public int CalculateVisibleDistance(Node node1, Node node2)
        {
            return ((node1.Position.X - node2.Position.X) * (node1.Position.X - node2.Position.X)) + ((node1.Position.Y - node2.Position.Y) * (node1.Position.Y - node2.Position.Y));
        }
    }
}
