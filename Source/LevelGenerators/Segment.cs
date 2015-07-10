using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;

namespace Turnable.LevelGenerators
{
    public class Segment
    {
        public Position Start { get; set; }
        public Position End { get; set; }

        public Segment(Position start, Position end)
        {
            if (start.X != end.X && start.Y != end.Y)
            {
                throw new InvalidOperationException();
            }

            Start = start;
            End = end;
        }
    }
}
